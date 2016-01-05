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
    /// <summary>
    /// ConfigurationElementBase
    /// </summary>
    [Serializable]
    public class ConfigurationElementBase:ConfigurationElement
    {
        #region Private Member

        /// <summary>
        /// true , require name; false, don't require name
        /// </summary>
        private bool m_NameRequired;

        /// <summary>
        /// Get the options
        /// </summary>
        private NameValueCollection Options;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigurationElementBase() : this(true)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationElementBase"/> class.
        /// </summary>
        /// <param name="nameRequired">if set to <c>true</c> [name required].</param>
        public ConfigurationElementBase(bool nameRequired)
        {
            m_NameRequired = nameRequired;
            Options = new NameValueCollection();
        }

        #endregion

        #region Public Member

        /// <summary>
        /// Gets the name
        /// </summary>
        [ConfigurationProperty("name")]
        public string Name
        {
            get { return this["name"] as string; }
        }

        /// <summary>
        /// Gets the option elements
        /// </summary>
        public NameValueCollection OptionElements { get; set; }

        #endregion


        #region Public Method

        /// <summary>
        /// Reads xml from the configuration file.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="serializeCollectionKey">The XmlReader that reads from the configuration file.</param>
        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            base.DeserializeElement(reader, serializeCollectionKey);

            if (m_NameRequired && string.IsNullOrEmpty(Name))
            {
                throw new ConfigurationErrorsException("Required attribute 'name' not found");
            }
        }

        /// <summary>
        /// Get a value indicating whether an unknown attribute is encountered during deserialization
        /// </summary>
        /// <param name="name">the name of the unrecognized attribute</param>
        /// <param name="value">The value of the unrecognized attribute</param>
        /// <returns>true when an unknown attribute is encountered while deserializing; other false</returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            Options.Add(name,value);
            return true;
        }

        /// <summary>
        /// Modifies the ConfigurationElement object to remove all values that should not be saved
        /// </summary>
        /// <param name="sourceElement">A ConfigurationElement at  the current level containing a merged view of the properties</param>
        /// <param name="parentElement">The parent ConfigurationElement, or null if this is the top level</param>
        /// <param name="saveMode">A ConfigurationSaveMode that determines which property values to include</param>
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
        /// Writes the contents of this configuration element to the configuration file when implemented in a derived class
        /// </summary>
        /// <param name="writer">the XmlWriter that writes to the configuration file</param>
        /// <param name="serializeCollectionKey">true to serialize only the collection key properties; otherwise, false;</param>
        /// <returns>true if any data was actually serialized; otherwise false.</returns>
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
                for (int i = 0; i < optionElements.Count; i++)
                {
                    writer.WriteRaw(optionElements.Get(i));
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a vluew indicating whether an unknown element is encountered during deserialization
        /// </summary>
        /// <param name="elementName">the name of unknown subelement</param>
        /// <param name="reader">the XmlReader being used for deserialization.</param>
        /// <returns>true when an unknown element is encountered while deserializing; otherwise,false.</returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (OptionElements == null)
            {
                OptionElements = new NameValueCollection();
            }

            OptionElements.Add(elementName,reader.ReadOuterXml());

            return true;
        }

        #endregion
    }
}
