namespace GeometryNodes
{
    internal class Descriptions
    {
        public const string ORIGINAL = "Object to duplicate";
        public const string PARENT = "Parent object created for the " +
            "original and its copies. Connect it to Original port of " +
            "the next Geometry Node.";

        public static string TransformCoordinate(
                string verb,
                string subject,
                char axis) =>
            $"{char.ToUpper(axis)}-{subject} which to {verb} the object, in local " +
            $"space of its parent. If unconnected it is not {verb}d on the " +
            $"{axis}-axis.";
    }
}
