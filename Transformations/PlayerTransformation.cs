namespace DBZMOD.Transformations
{
    public class PlayerTransformation
    {
        public PlayerTransformation(TransformationDefinition transformationDefinition, float mastery)
        {
            TransformationDefinition = transformationDefinition;
            Mastery = mastery;
        }

        public TransformationDefinition TransformationDefinition { get; private set; }

        public float Mastery { get; private set; }
    }
}
