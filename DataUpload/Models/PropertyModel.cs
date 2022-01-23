using Nest;
using System.Collections.Generic;


namespace DataUpload.Models
{
    public class PropertyModel
    {
        public Property property { get; set; }
    }

    //Added to make the propertyID property the id when uploaded to Elasticsearch
    [ElasticsearchType(IdProperty = nameof(propertyID))]
    public class Property
    {
        public int propertyID { get; set; }
        public string name { get; set; }
        public string formerName { get; set; }
        public string streetAddress { get; set; }
        public string city { get; set; }
        public string market { get; set; }
        public string state { get; set; }
        public float lat { get; set; }
        public float lng { get; set; }
    }
}