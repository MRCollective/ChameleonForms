using System;
using System.Collections.Generic;
using ChameleonForms.Example.Controllers;

namespace ChameleonForms.AcceptanceTests.Helpers
{
    public static class ObjectMother
    {
        public static class ModelBindingViewModels
        {
            public static ModelBindingViewModel BasicValid
            {
                get
                {
                    return new ModelBindingViewModel
                    {
                        RequiredString = "RequiredString",
                        RequiredInt = 1,
                        OptionalInt = null,
                        RequiredBool = true,
                        RequiredNullableBool = false,
                        OptionalBool = true,
                        OptionalBool2 = false,
                        OptionalBool3 = null,
                        RequiredEnum = SomeEnum.SomeOtherValue,
                        RequiredNullableEnum = SomeEnum.Value1,
                        OptionalEnum = null,
                        RequiredEnums = new List<SomeEnum> { SomeEnum.ValueWithDescription, SomeEnum.SomeOtherValue },
                        RequiredNullableEnums = new List<SomeEnum?> { SomeEnum.Value1 },
                        OptionalEnums = null,
                        OptionalNullableEnums = new List<SomeEnum?>(),
                        RequiredListId = 1,
                        OptionalListId = null,
                        RequiredNullableListId = 1,
                        RequiredListIds = new List<int> {1},
                        RequiredNullableListIds = new List<int?> {1},
                        //OptionalListIds = null,
                        OptionalNullableListIds = null,
                        DateTime = new DateTime(2010, 1, 13)
                    };
                }
            }
        }
    }
}
