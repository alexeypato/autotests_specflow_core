using TechTalk.SpecFlow;

namespace ProjectTests.Steps.Transformations
{
    [Binding]
    public class StepArgumentTransformations
    {
        [StepArgumentTransformation(@"(contains|doesn't contain)")]
        public bool DoesContain(string value)
        {
            return value.Equals("contains");
        }

        [StepArgumentTransformation(@"(display|hide)")]
        public bool DoesDisplay(string value)
        {
            return value.Equals("display");
        }

        [StepArgumentTransformation(@"(select|unselect)")]
        public bool DoesSelect(string value)
        {
            return value.Equals("select");
        }

        [StepArgumentTransformation(@"(displayed|hidden)")]
        public bool IsDisplayed(string value)
        {
            return value.Equals("displayed");
        }

        [StepArgumentTransformation(@"(existed|removed)")]
        public bool IsExisted(string value)
        {
            return value.Equals("existed");
        }

        [StepArgumentTransformation(@"(enabled|disabled)")]
        public bool IsEnabled(string value)
        {
            return value.Equals("enabled");
        }

        [StepArgumentTransformation(@"(selected|unselected)")]
        public bool IsSelected(string value)
        {
            return value.Equals("selected");
        }
    }
}
