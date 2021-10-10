
public static partial class Tutorials
{
    public static void TryStartTutorials()
    {
        if (!FirstStartTutorial.IsCompleted)
            FirstStartTutorial.Start(TryStartTutorials);
        else if (!AchievementTutorial.IsCompleted)
            AchievementTutorial.Start(TryStartTutorials);
        else if (!CollectionTutorial.IsCompleted)
            CollectionTutorial.Start(TryStartTutorials);
        else if (!ShopTutorial.IsCompleted)
            ShopTutorial.Start(TryStartTutorials);
        else if (!BoosterTutorial.IsCompleted)
            BoosterTutorial.Start();
    }
    
    public static TutorialDataModel<FirstStartTutorialAction> FirstStartTutorial 
        => firstStartTutorial ?? (firstStartTutorial = new TutorialDataModel<FirstStartTutorialAction>("first_start"));
    
    public static TutorialDataModel<AchievementTutorialAction> AchievementTutorial 
        => achievementTutorial ?? (achievementTutorial = new TutorialDataModel<AchievementTutorialAction>("achievement"));
    
    public static TutorialDataModel<CollectionTutorialAction> CollectionTutorial 
        => collectionTutorial ?? (collectionTutorial = new TutorialDataModel<CollectionTutorialAction>("collection"));
    
    public static TutorialDataModel<ShopTutorialAction> ShopTutorial 
        => shopTutorial ?? (shopTutorial = new TutorialDataModel<ShopTutorialAction>("shop"));
    
    public static TutorialDataModel<BoosterTutorialAction> BoosterTutorial 
        => boosterTutorial ?? (boosterTutorial = new TutorialDataModel<BoosterTutorialAction>("booster"));
    
    static TutorialDataModel<FirstStartTutorialAction> firstStartTutorial;
    static TutorialDataModel<AchievementTutorialAction> achievementTutorial;
    static TutorialDataModel<CollectionTutorialAction> collectionTutorial;
    static TutorialDataModel<ShopTutorialAction> shopTutorial;
    static TutorialDataModel<BoosterTutorialAction> boosterTutorial;
}
