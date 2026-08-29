namespace webapikits.Model
{
    public class RowLevelSecurityModel
    {
        public string ClassName { get; set; } = "";

        public string FieldName { get; set; } = "";

        public string FieldValue { get; set; } = "";

        public bool Active { get; set; }
    }

}
