namespace GedcomParser.Entities
{
    public class Citation
    {
        public string Id { get; set; }
        public string Page { get; set; }
        public DataQuality CertaintyAssessment { get; set; }
        public enum DataQuality
        {
            Unknown = -1,
            Unreliable = 0, // Unreliable evidence or estimated data
            Questionable = 1, // Questionable reliability of evidence
                              // (interviews, census, oral genealogies,
                              // or potential for bias for example, an autobiography)
            Reliable = 2, // Secondary evidence, data officially recorded sometime after event
            Primary = 3, // Direct and primary evidence used, or by dominance of the evidence
        }
    }
}
