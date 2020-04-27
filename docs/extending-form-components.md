# Extending Form Components

If you would like to extend form components you can easily create extension methods on the relevant classes.

The `FieldFor` extension methods on the `Section` class [are a great example](https://github.com/MRCollective/ChameleonForms/blob/master/ChameleonForms/Component/Field.cs#L71).

There is nothing stopping you creating similar methods, or even creating your own form components by extending the `IFormComponent` interface or the `FormComponent` class.

If you have a specific DSL you would like to achieve using ChameleonForms and need some assistance feel free to communicate with us via Twitter [@robdmoore](http://twitter.com/robdmoore) / [@mdaviesnet](http://twitter.com/mdaviesnet) or alternatively send a [pull request](https://github.com/MRCollective/ChameleonForms/pulls) / [issue](https://github.com/MRCollective/ChameleonForms/issues) to the [GitHub project](https://github.com/MRCollective/ChameleonForms).
