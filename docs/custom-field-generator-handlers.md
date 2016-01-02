Custom Field Generator Handlers
===============================

If you want to use your own custom field generator handlers combined or in-place with the built-in handlers then you will need to firstly create a custom [Field Generator](custom-field-generator), then inside of that delegate to a different [FieldGeneratorHandlersRouter](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/FieldGenerators/FieldGeneratorHandlersRouter.cs) that has a different set of handlers.