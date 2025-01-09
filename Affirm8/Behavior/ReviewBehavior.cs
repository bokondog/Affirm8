using Affirm8.Common;
using Affirm8.Views.Feedback;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Toolkit.Buttons;

namespace Affirm8
{
    public class ReviewBehavior : Behavior<Review>
    {
        /// <summary>
        /// The data form
        /// </summary>
        private SfDataForm? reviewForm;

        /// <summary>
        /// The submit button
        /// </summary>
        private SfButton? submitButton;

        protected override void OnAttachedTo(Review bindable)
        {
            base.OnAttachedTo(bindable);
            Review? reviewPage = bindable as Review;
            if (reviewPage == null)
            {
                return;
            }

            this.reviewForm = (SfDataForm)reviewPage.Content.FindByName("reviewForm");
            if (this.reviewForm != null)
            {
                reviewForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButton = (SfButton)reviewPage.Content.FindByName("submitButton");
            if (this.submitButton != null)
            {
                this.submitButton.Clicked += OnAddButtonClicked;
            }
        }

        protected override void OnDetachingFrom(Review bindable)
        {
            base.OnDetachingFrom(bindable);
            Review? reviewPage = bindable as Review;
            if (reviewPage == null)
            {
                return;
            }

            this.reviewForm = (SfDataForm)reviewPage.Content.FindByName("reviewForm");
            if (this.reviewForm != null)
            {
                reviewForm.ItemsSourceProvider = new ItemsSourceProvider();
            }

            this.submitButton = (SfButton)reviewPage.Content.FindByName("submitButton");
            if (this.submitButton != null)
            {
                this.submitButton.Clicked -= OnAddButtonClicked;
            }
        }

        private void OnAddButtonClicked(object? sender, EventArgs e)
        {
            this.reviewForm?.Validate();
        }
    }
}
