using System.Collections.Generic;
using Puzzle;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public class CollectionWizardCompletePage : WizardPage
{
    public override string Title => "Summary";
    public override string Description => "Check that all things you've done right";


    string Name;
    bool DefaultUnlocked;
    PuzzleColorData[] PuzzleColors;
    CollectionData CollectionData;
    int AnimationVariants;

    public override void Enter()
    {
        base.Enter();
        Name = Wizard.GetData<string>("collection_wizard_name");
        DefaultUnlocked = Wizard.GetData<bool>("collection_wizard_default_unlocked");
        PuzzleColors = Wizard.GetData<PuzzleColorData[]>("collection_wizard_color_data");
        CollectionData = Wizard.GetData<CollectionData>("collection_wizard_collection_data");
        AnimationVariants = Wizard.GetData<int>("collection_wizard_animations_variants");
    }


    public override void Draw(Rect rect)
    {
        GUILayout.BeginArea(rect);
        GUILayout.BeginVertical();

        EditorGUI.LabelField(rect, $"You're about creating puzzle with name(id) {Name}," +
                                  $" it will be unlocked for player by default: {DefaultUnlocked}.\n " +
                                  $"And the puzzle will have {PuzzleColors.Length} different colors.\n " +
                                  $"Puzzle will have {AnimationVariants} animation variants. \n" + 
                                   "If all right press \"FINISH\"", Wizard.BlackLabel);
        
        GUILayout.EndVertical();
        GUILayout.EndArea();

        if (DrawFinishButton(rect))
        {
            Execute();
            Wizard.Close();
        }
    }
    
    void Execute()
    {
        CollectionItem collectionItem = CreateCollectionItem();

        CreateContent(collectionItem);
    }

    CollectionItem CreateCollectionItem()
    {
        CollectionItem collectionItem = ScriptableObject.CreateInstance<CollectionItem>();
        
        collectionItem.Name = Name;
        collectionItem.puzzleColors = PuzzleColors;
        collectionItem.defaultUnlocked = DefaultUnlocked;
        
        AssetDatabase.CreateAsset(collectionItem, $"Assets/Data/Account/Collection/CollectionItems/{Name}CollectionItem.asset");
        CollectionData.AddCollectionItem(collectionItem);
        
        AssetDatabase.Refresh();

        return collectionItem;
    }
    
    void CreateContent(CollectionItem collectionItem)
    {
        const string pathToCollectionFolder = "Assets/CollectionPuzzles";

        string pathToPuzzleFolder = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(pathToCollectionFolder, collectionItem.Name));
        string pathToAnimations = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(pathToPuzzleFolder, "Animations"));
        string pathToImages = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(pathToPuzzleFolder, "Images"));
        string pathToPrefabs = AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder(pathToPuzzleFolder, "Prefabs"));

        AnimatorController controller = CreateAnimations(pathToAnimations);
        CreatePrefabs(pathToPrefabs, controller, collectionItem);
    }

    void CreatePrefabs(string prefabsFolder, AnimatorController controller, CollectionItem collectionItem)
    {
        Sprite[] sprites = GetShapeVariants(new PuzzleSides(true, true, true, true));

        GameObject puzzleObject = new GameObject($"{Name}_TRBL");
        
        Rigidbody2D rigidbody = puzzleObject.AddComponent<Rigidbody2D>();
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        BoxCollider2D collider = puzzleObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        //empirical parameter
        collider.offset = new Vector2(0.02280325f, 0.01400089f);
        collider.size = new Vector2(2.17968f, 2.267714f);

        PlayerView playerView = puzzleObject.AddComponent<SkinPlayerView>();
        
        Animator animator = puzzleObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = controller;

        RandomAnimatorParameter.SetSettings(puzzleObject.AddComponent<RandomAnimatorParameter>(), AnimationVariants);

        SortingGroup sortingGroup = puzzleObject.AddComponent<SortingGroup>();
        sortingGroup.sortingLayerName = "Player";
        sortingGroup.sortingOrder = 1;
        
        GameObject puzzleShape = new GameObject("Shape");
        puzzleShape.transform.parent = puzzleObject.transform;
        playerView.shape = puzzleShape.transform;

        SpriteRenderer spriteRenderer = puzzleShape.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];
        spriteRenderer.sortingLayerName = "Player";
        
        SkinContainer skinContainer = puzzleShape.AddComponent<SkinContainer>();
        SkinContainer.SetEditorSprites(skinContainer, sprites);

        PlayerViewColorSkin playerViewColorSkin = puzzleShape.AddComponent<PlayerViewColorSkin>();
        PlayerView.SetEditorColorSkins(playerView, new []{playerViewColorSkin});
        
        List<PlayerViewColorSkin.ColorSkin> colorSkins = new List<PlayerViewColorSkin.ColorSkin>();
        
        foreach (PuzzleColorData puzzleColor in PuzzleColors)
        {
            PlayerViewColorSkin.ColorSkin colorSkin = new PlayerViewColorSkin.ColorSkin{Color = puzzleColor.Color, PuzzleColor = puzzleColor};
            colorSkins.Add(colorSkin);
        }

        playerViewColorSkin.EditorColorSkins = colorSkins.ToArray();
        
        Transform top = new GameObject("top").transform;
        top.parent = puzzleObject.transform;
        top.localPosition = new Vector3(0, 2, 0);
        
        Transform right = new GameObject("right").transform;
        right.parent = puzzleObject.transform;
        right.localPosition = new Vector3(2, 0, 0);
        
        Transform bottom = new GameObject("bottom").transform;
        bottom.parent = puzzleObject.transform;
        bottom.localPosition = new Vector3(0, -2, 0);
        
        Transform left = new GameObject("left").transform;
        left.parent = puzzleObject.transform;
        left.localPosition = new Vector3(-2, 0, 0);
        
        PlayerView.SetEditorTRBL(playerView, new []{top, right, bottom, left});

        GameObject prefabTRBL = PrefabUtility.SaveAsPrefabAsset(puzzleObject, prefabsFolder + $"/{Name}_TRBL.prefab");
        
        List<PuzzleVariant> puzzleVariants = new List<PuzzleVariant>(4);
        
        PuzzleSides[] targetSides = new[]
        {
            //we don't need all sides (TRBL) puzzle because it's already created
            new PuzzleSides(true, false, true, false),
            new PuzzleSides(true, false, false, false),
            new PuzzleSides(true, false, false, true),
        };

        puzzleVariants.Add(new PuzzleVariant(new PuzzleSides(true, true, true, true), prefabTRBL));
        
        foreach (PuzzleSides sides in targetSides)
            puzzleVariants.Add(CreatePuzzleVariant(sides, prefabTRBL, prefabsFolder));
        
        CollectionItem.SetEditorPuzzleVariants(collectionItem, puzzleVariants.ToArray());
        
        AssetDatabase.Refresh();
        
        Object.DestroyImmediate(puzzleObject);
    }
    
    PuzzleVariant CreatePuzzleVariant(PuzzleSides sides, GameObject originalPrefab, string prefabsFolder)
    {
        GameObject variantInstance = (GameObject) PrefabUtility.InstantiatePrefab(originalPrefab);

        Sprite[] sprites = GetShapeVariants(sides);
        
        Transform shape = variantInstance.transform.Find("Shape");
        
        SpriteRenderer spriteRenderer = shape.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[0];

        SkinContainer skinContainer = shape.GetComponent<SkinContainer>();
        SkinContainer.SetEditorSprites(skinContainer, sprites);
        
        GameObject prefabVariant = PrefabUtility.SaveAsPrefabAsset(variantInstance, prefabsFolder + $"/{Name}_{sides.ToString()}.prefab");
        
        Object.DestroyImmediate(variantInstance);
        
        return new PuzzleVariant(sides, prefabVariant);
    }
    
    AnimatorController CreateAnimations(string animationsFolder)
    {
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath($"{animationsFolder}/{Name}_animator.controller");
        
        AssetDatabase.Refresh();

        const string randomParameter = "RandomParameter";
        const string damagedParameter = "Damaged";
        
        // Add parameters
        controller.AddParameter(randomParameter, AnimatorControllerParameterType.Int);
        controller.AddParameter(damagedParameter, AnimatorControllerParameterType.Trigger);

        // Add StateMachines
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;

        // Add States
        AnimatorState stateIdle = rootStateMachine.AddState("idle");
        AnimatorState stateDamaged = rootStateMachine.AddState("damaged");

        rootStateMachine.defaultState = stateIdle;


        for (int i = 0; i < AnimationVariants; i++)
        {
            AnimatorState stateVariant = rootStateMachine.AddState($"variation_{i}");
            stateVariant.AddStateMachineBehaviour<AnimationEventBehaviour>();
            
            AnimatorStateTransition stateToExitTransition = stateVariant.AddExitTransition();
            stateToExitTransition.hasExitTime = true;
            stateToExitTransition.exitTime = 1;
            stateToExitTransition.hasFixedDuration = false;
            stateToExitTransition.duration = 0;
            
            AnimatorStateTransition idleToStateTransition = stateIdle.AddTransition(stateVariant);
            idleToStateTransition.hasExitTime = true;
            idleToStateTransition.exitTime = 1;
            idleToStateTransition.hasFixedDuration = false;
            idleToStateTransition.duration = 0;
            idleToStateTransition.AddCondition(AnimatorConditionMode.Equals, i, randomParameter);
            
            AnimationClip stateAnim = new AnimationClip {name = $"{Name}_variant_{i}"};
            AssetDatabase.AddObjectToAsset(stateAnim, controller);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(stateAnim));

            stateVariant.motion = stateAnim;
        }
        
        // Add Transitions
        AnimatorStateTransition stateDamagedExitTransition = stateDamaged.AddExitTransition();
        stateDamagedExitTransition.hasExitTime = true;
        stateDamagedExitTransition.exitTime = 1;
        stateDamagedExitTransition.hasFixedDuration = false;
        stateDamagedExitTransition.duration = 0;

        AnimatorStateTransition anyStateDamagedTransition = rootStateMachine.AddAnyStateTransition(stateDamaged);
        anyStateDamagedTransition.canTransitionToSelf = true;
        anyStateDamagedTransition.hasExitTime = true;
        anyStateDamagedTransition.exitTime = 1;
        anyStateDamagedTransition.hasFixedDuration = false;
        anyStateDamagedTransition.duration = 0;
        anyStateDamagedTransition.AddCondition(AnimatorConditionMode.If, 1, damagedParameter);

        //Create animation clips
        
        AnimationClip idleAnim = new AnimationClip {name = $"{Name}_idle"};
        AnimationUtility.SetAnimationClipSettings(idleAnim, new AnimationClipSettings {loopTime = true});
        
        AnimationClip stateDamagedAnim = new AnimationClip {name = $"{Name}_damaged"};
        
        AssetDatabase.AddObjectToAsset(idleAnim, controller);
        AssetDatabase.AddObjectToAsset(stateDamagedAnim, controller);
        
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(idleAnim));
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(stateDamagedAnim));
        
        //Bind clips to states
        stateDamaged.motion = stateDamagedAnim;
        stateIdle.motion = idleAnim;
        
        AssetDatabase.Refresh();

        return controller;
    }

    Sprite[] GetShapeVariants(PuzzleSides sides)
    {
        string shapes = sides.ToString();

        return new[]
        {
            AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Data/Images/Puzzles/BasePuzzle/Base_{shapes}/base_state_{shapes}_{1}.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Data/Images/Puzzles/BasePuzzle/Base_{shapes}/base_state_{shapes}_{2}.png")
        };
    }

    
    
}
