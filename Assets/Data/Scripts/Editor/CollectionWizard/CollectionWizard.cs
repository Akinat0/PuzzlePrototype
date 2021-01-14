public class CollectionWizard : Wizard
{
    readonly WizardPage[] pages =
    {
        new CollectionWizardFirstPage(),
        new CollectionWizardParametersPage(),
        new CollectionWizardAnimationStates(),
        new CollectionWizardCompletePage()
    };
    
    protected override WizardPage[] Pages => pages;
}
