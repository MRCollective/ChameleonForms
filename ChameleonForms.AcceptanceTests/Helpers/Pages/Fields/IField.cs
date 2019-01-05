namespace ChameleonForms.AcceptanceTests.ModelBinding.Pages.Fields
{
    internal interface IField
    {
        //void Set(IModelFieldValue value);
        object Get(IModelFieldType fieldType);
    }

    internal class NoField : IField
    {
        //public void Set(IModelFieldValue value) {}

        public object Get(IModelFieldType fieldType)
        {
            return fieldType.DefaultValue;
        }
    }
}
