public class ComponentTemplate
{
    public const string ClassTemplate =
        "public class #COMPONENT_NAME# : Entitas.IComponent\r" +
        "\n" +
        "{" +
        "\r\n" +
        "#FIELDS#" +
        "}";

    public const string FieldTemplate = "    public #FIELD_TYPE# #FIELD_NAME#;\r\n";
}
