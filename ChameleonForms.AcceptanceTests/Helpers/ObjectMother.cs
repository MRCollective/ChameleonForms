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
                        RequiredFlagsEnum = FlagsEnum.Four | FlagsEnum.Two,
                        RequiredNullableFlagsEnum = FlagsEnum.Two | FlagsEnum.Five,
                        OptionalFlagsEnum = null,
                        RequiredEnums = new List<SomeEnum> { SomeEnum.ValueWithDescription, SomeEnum.SomeOtherValue },
                        RequiredNullableEnums = new List<SomeEnum?> { SomeEnum.Value1 },
                        OptionalEnums = new List<SomeEnum>(),
                        OptionalNullableEnums = new List<SomeEnum?> { null },
                        RequiredListId = 1,
                        OptionalListId = null,
                        RequiredNullableListId = 1,
                        RequiredListIds = new List<int> {1},
                        RequiredNullableListIds = new List<int?> {1},
                        //OptionalListIds = null,
                        OptionalNullableListIds = null,
                        DateTime = new DateTime(2010, 1, 13),
                        Choice = 1
                    };
                }
            }
        }

        public static class ChangingContextViewModels
        {
            public static BasicViewModel DifferentViewModel
            {
                get
                {
                    return new BasicViewModel
                    {
                        RequiredString = "req_string",
                        SomeCheckbox = true
                    };
                }
            }

            public static ChildViewModel ChildViewModel
            {
                get
                {
                    return new ChildViewModel
                    {
                        ChildField = "child_field_val",
                        SomeEnum = SomeEnum.ValueWithDescription
                    };
                }
            }

            public static ParentViewModel ParentViewModel
            {
                get
                {
                    return new ParentViewModel
                    {
                        Child = new ChildViewModel
                        {
                            ChildField = "child_field_val_from_parent",
                            SomeEnum = SomeEnum.SomeOtherValue
                        }
                    };
                }
            }
        }
    }
}
