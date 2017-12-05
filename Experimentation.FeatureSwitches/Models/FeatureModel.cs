using System.Collections.Generic;

namespace Experimentation.FeatureSwitches.Models
{
    public class FeatureModel
    {
        public string Id { get; set; }
        public int FriendlyId { get; set; }
        public string Name { get; set; }
        public List<string> BucketList { get; set; }
    }
}