using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SuperSocket.Common
{
    [Serializable]
    public class ConfigurationElementBase:ConfigurationElement
    {
        /// <summary>
        /// 配置项是否需要命名
        /// </summary>
        private bool nameRequired;

        #region Public Properties

        /// <summary>
        /// Get the options
        /// </summary>
        public NameValueCollection Options { get; set; }

        /// <summary>
        /// Gets the option elements.
        /// </summary>
        public NameValueCollection OptionElements { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigurationElementBase():this(true)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nameRequired">if set to <c>true</c> [name required].</param>
        public ConfigurationElementBase(bool nameRequired)
        {
            this.nameRequired = nameRequired;
            Options = new NameValueCollection();
        }

        /// <summary>
        /// get the name
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"] as string; }
        }

        /// <summary>
        /// Reads XML from the configuration file.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> that reads from the configuration file.</param>
        /// <param name="serializeCollectionKey">true to serialize only the collection key properties; otherwise, false.</param>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">The element to read is locked.- or -An attribute of the current node is not recognized.- or -The lock status of the current node cannot be determined.  </exception>
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);

            if (nameRequired && string.IsNullOrEmpty(Name))
            {
                throw new ConfigurationErrorsException("Required attribute 'name' not found.");
            }
        }

        /// <summary>
        /// Get a value indicating whether an unknown attribute is encountered during deserialization
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            Options.Add(name,value);
            return true;
        }

        /// <summary>
        /// Modifies the System.Configuration.ConfigurationElement object to remove all values that should not be saved.
        /// </summary>
        /// <param name="sourceElement"></param>
        /// <param name="parentElement"></param>
        /// <param name="saveMode"></param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            base.Unmerge(sourceElement, parentElement, saveMode);

            var element = sourceElement as ConfigurationElementBase;

            if (element == null)
            {
                return;
            }

            if (element.Options != this.Options)
            {
                this.Options = element.Options;
            }

            if (element.OptionElements != this.OptionElements)
            {
                this.OptionElements = element.OptionElements;
            }
        }

        /// <summary>
        /// Writes the contents of this configuration element to the configuration file when implemented in a derived class.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="serializeCollectionKey"></param>
        /// <returns></returns>
        protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
        {
            if (!base.SerializeElement(writer, serializeCollectionKey))
            {
                return false;
            }

            var options = Options;
            if (options != null && options.Count > 0)
            {
                for (int i = 0; i < options.Count; i++)
                {
                    writer.WriteAttributeString(options.GetKey(i),options.Get(i));
                }
            }

            var optionElements = OptionElements;

            if (optionElements != null && optionElements.Count > 0)
            {
                for (var i = 0; i < optionElements.Count; i++)
                {
                    writer.WriteRaw(optionElements.Get(i));
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether an unknown element is encountered during deserialization.
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (OptionElements == null)
            {
                OptionElements = new NameValueCollection();
            }

            OptionElements.Add(elementName,reader.ReadOuterXml());
            return true;
        }
    }
}
