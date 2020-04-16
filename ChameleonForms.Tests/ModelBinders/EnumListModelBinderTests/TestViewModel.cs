using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChameleonForms.Tests.ModelBinders.EnumListModelBinderTests
{
    public class TestViewModel
    {
        public const string EmptySubmission = "";
        public const string InvalidSingleSubmission = "Invalid";
        public static readonly string[] InvalidMultipleSubmission = { "Two", "Invalid" };
        public static readonly IEnumerable<TestEnum> PartialListFromInvalidMultipleSubmission = new[] { TestEnum.Two };
        public static readonly IEnumerable<TestEnum?> PartialNullableListFromInvalidMultipleSubmission = new TestEnum?[] { TestEnum.Two };
        public const string ValidSingleSubmission = "One";
        public static readonly IEnumerable<TestEnum> ListFromValidSingleSubmission = new[] { TestEnum.One };
        public static readonly IEnumerable<TestEnum?> NullableListFromValidSingleSubmission = new TestEnum?[] { TestEnum.One };
        public static readonly string[] ValidMultipleSubmission = { "One", "Two" };
        public static readonly IEnumerable<TestEnum> ListFromValidMultipleSubmission = new[] { TestEnum.One, TestEnum.Two };
        public static readonly IEnumerable<TestEnum?> NullableListFromValidMultipleSubmission = new TestEnum?[] { TestEnum.One, TestEnum.Two };
        public static readonly string[] ValidMultipleSubmissionWithEmptyValue = { "", "One", "Two" };


        [Required]
        public IEnumerable<TestEnum?> RequiredNullableEnumList { get; set; }
        public IEnumerable<TestEnum?> OptionalNullableEnumList { get; set; }
        [Required]
        public IEnumerable<TestEnum> RequiredEnumList { get; set; }
        public IEnumerable<TestEnum> OptionalEnumList { get; set; }

        public enum TestEnum
        {
            One,
            Two
        }
    }
}