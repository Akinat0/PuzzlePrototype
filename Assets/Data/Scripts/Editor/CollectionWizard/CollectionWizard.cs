public class CollectionWizard : Wizard
{
    readonly WizardPage[] pages =
    {
        new CollectionWizardFirstPage(),
        new CollectionWizardParametersPage(), 
        new CollectionWizardCompletePage()
    };
    protected override WizardPage[] Pages => pages;
}
