# Custom Field Generator

If the [Default Field Generator](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/FieldGenerators/DefaultFieldGenerator.cs) doesn't cut it for you then feel free to create your own field generator and use it instead by creating your own [Form](the-form.md) class and overriding the `GetFieldGenerator` method.

You will need to create your [own extension method in place of `BeginChameleonForm`](form-templates.md#custom-extension-method) to new up your custom `Form` class.
