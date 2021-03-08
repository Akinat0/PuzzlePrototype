#import <Foundation/Foundation.h>

UISelectionFeedbackGenerator*    SelectionGenerator;
UINotificationFeedbackGenerator* NotificationGenerator;
UIImpactFeedbackGenerator*       LightImpactGenerator;
UIImpactFeedbackGenerator*       MediumImpactGenerator;
UIImpactFeedbackGenerator*       HeavyImpactGenerator;

void InitializeHapticGenerators()
{
    SelectionGenerator    = [[UISelectionFeedbackGenerator alloc] init];
    NotificationGenerator = [[UINotificationFeedbackGenerator alloc] init];
    LightImpactGenerator  = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleLight];
    MediumImpactGenerator = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleMedium];
    HeavyImpactGenerator  = [[UIImpactFeedbackGenerator alloc] initWithStyle:UIImpactFeedbackStyleHeavy];
}

void HapticSelection()
{
    [SelectionGenerator selectionChanged];
}

void HapticSuccess()
{
    [NotificationGenerator notificationOccurred:UINotificationFeedbackTypeSuccess];
}

void HapticWarning()
{
    [NotificationGenerator notificationOccurred:UINotificationFeedbackTypeWarning];
}

void HapticFailure()
{
    [NotificationGenerator notificationOccurred:UINotificationFeedbackTypeError];
}

void HapticLightImpact()
{
    [LightImpactGenerator impactOccurred];
}

void HapticMediumImpact()
{
    [MediumImpactGenerator impactOccurred];
}

void HapticHeavyImpact()
{
    [HeavyImpactGenerator impactOccurred];
}
