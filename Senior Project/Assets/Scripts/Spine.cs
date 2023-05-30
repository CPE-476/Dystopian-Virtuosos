using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public enum InterfaceState
{
    GAMEPLAY,
    DIALOGUE,
    TUTORIAL,
    GAME_OVER
}

public enum GameplayAudio
{
    DRUMS,
    PIANO,
    GUITAR,
    S1,
    S2,
    END,
}

public enum Character
{
    RIKA,
    BRONTE,
    THREE,
    SHOPKEEPER
}

public class DialogueLine
{
    public Character character;

    public string line;

    public DialogueLine(Character char_in, string line_in)
    {
        character = char_in;
        line = line_in;
    }
}

public class TutorialClip
{
    public string name;

    public string video_path;

    public TutorialClip(string name_in, string video_in)
    {
        name = name_in;
        video_path = video_in;
    }
}

public struct Section
{
    public DialogueLine[] conversation;

    public TutorialClip[] tutorialVideos;

    public string midi_path;

    public GameplayAudio audio_to_play;

    public int l2_background_audio;

    public bool background_audio;

    public int beats_till_first_note;

    public Section(
        DialogueLine[] conv_in,
        TutorialClip[] video_in,
        string midi_in,
        GameplayAudio audio_in,
        int l2_background_audio_in,
        bool background_audio_in,
        int beats_till_in
    )
    {
        conversation = conv_in;
        tutorialVideos = video_in;
        midi_path = midi_in;
        audio_to_play = audio_in;
        l2_background_audio = l2_background_audio_in;
        background_audio = background_audio_in;
        beats_till_first_note = beats_till_in;
    }
}

public class Spine : MonoBehaviour
{
    public int section_index = -1;

    public Section[] sections;

    public InterfaceState state = InterfaceState.DIALOGUE;

    public Conductor conductor;

    public MIDIReader midiReader;

    public CameraController cam;

    public Dialogue dialogue;

    public NoteTrigger noteTrigger;

    public HealthBar healthBar;

    public ScoreManager scoreManager;

    public ComboManager comboManager;

    public GameObject fade;

    public Tutorial tutorial;

    private Image image;

    private Color color;

    public DialogueLine[]
        first_dialogue =
        {
            new DialogueLine(Character.RIKA,
                "Welcome to Virtuosos!"),
            new DialogueLine(Character.RIKA,
                "This is an early demo, so give whatever feedback you can. So let's get started!"),
            new DialogueLine(Character.RIKA,
                "There are two kinds of input: Keyboard and Controller."),
            new DialogueLine(Character.RIKA,
                "If you're using the controller, hit the low track with Left Bumper, and the high track with Right Bumper. Don't overthink it!"),
            new DialogueLine(Character.RIKA,
                "On Keyboard, L hits the low track, and K hits the high track."),
            new DialogueLine(Character.RIKA,
                "Watch out! Up ahead, looks like there's some obstacles."),
            new DialogueLine(Character.RIKA,
                "Rocks can hurt you, so avoid them."),
            new DialogueLine(Character.RIKA,
                "You'll want to hit the enemies on the beat, and if you hit them right, you get points!"),
            new DialogueLine(Character.RIKA,
                "Finally, if you see screws, try to collect them."),
            new DialogueLine(Character.RIKA, "This is a rhythm game, so listen to the rhythms to know when to hit your targets."),
            new DialogueLine(Character.RIKA, "Do your best!")
        };

    public DialogueLine[]
        second_dialogue =
        {
            new DialogueLine(Character.RIKA,
                "Well done! That was the drummer mode."),
            new DialogueLine(Character.RIKA,
                "Next, we'll switch to another game mode: piano."),
            new DialogueLine(Character.RIKA,
                "The piano mode has hold notes. When you see a hold note, try to hit it on time, and hold it for as long as its tail lasts."),
            new DialogueLine(Character.RIKA,
                "Keep in mind, you might need to hold notes while other notes are played, so stay sharp! (haha)"),
            new DialogueLine(Character.RIKA,
                "The other thing is that the piano mode has four tracks. On Controller, you hit them with the left and right joysticks. Try it out before the notes start coming!"),
            new DialogueLine(Character.RIKA,
                "If you're on keyboard, you'll input using H, J, K, and L."),
            new DialogueLine(Character.RIKA,
                "Alright, here they come! Good luck!")
        };

    public DialogueLine[]
        third_dialogue =
        {
            new DialogueLine(Character.RIKA,
                "Excellent!"),
            new DialogueLine(Character.RIKA,
                "Time for the final showdown. This time, we'll be in the last game mode: Guitar!"),
            new DialogueLine(Character.RIKA,
                "Guitar mode is a combination of piano and drum modes. There are three tracks, and you can encounter both hit and hold notes at the same time."),
            new DialogueLine(Character.RIKA,
                "On controller, use the Y, B, and A buttons to hit and hold notes."),
            new DialogueLine(Character.RIKA,
                "On keyboard, it's the same as piano, just no top track."),
            new DialogueLine(Character.RIKA,
                "Alright, here we go!")
        };

    public DialogueLine[]
        fourth_dialogue = { new DialogueLine(Character.RIKA, "That's it.") };

    public TutorialClip[]
        first_tutorial = {
            new TutorialClip("OBSTACLE",
            "Assets/videos/test_1.mp4"),
            new TutorialClip("NOTE",
            "Assets/videos/test_2.mp4"),
            new TutorialClip("CONTROL",
            "Assets/videos/test_3.mp4"),
        };

    public TutorialClip[]
        second_tutorial = {

        };

    public TutorialClip[]
        third_tutorial = {

        };

    void Start()
    {
        sections =
            new Section[6]
            {
                new Section(second_dialogue, second_tutorial,
                    "Assets/Music/DV_L1_piano.mid",
                    GameplayAudio.PIANO,
                    0,
                    false,
                    -17),
                new Section(first_dialogue, first_tutorial,
                    "Assets/Music/DV_L2_Section_1.mid",
                    GameplayAudio.S1,
                    1,
                    false,
                    -17),
                new Section(first_dialogue, first_tutorial,
                    "Assets/Music/DV_L2_Section_2.mid",
                    GameplayAudio.S2,
                    2,
                    false,
                    -1),
                new Section(first_dialogue, first_tutorial,
                    "Assets/Music/DV_L2_Section_2.mid",
                    GameplayAudio.END,
                    3,
                    false,
                    -17),
                new Section(third_dialogue, third_tutorial,
                    "Assets/Music/DV_L1_guitar.mid",
                    GameplayAudio.GUITAR,
                    0,
                    true,
                    -17),
                new Section(first_dialogue, first_tutorial,
                    "Assets/Music/DV_L1_drum.mid",
                    GameplayAudio.DRUMS,
                    0,
                    true,
                    -1),
            };

        image = fade.GetComponent<Image>();
        color = image.color;

        DialogueStart();
    }

    public void DialogueStart()
    {
        color.a = 0.4f;
        image.color = color;
        healthBar.showHealthBar = false;
        scoreManager.showScoreBar = false;
        comboManager.showComboBar = false;
        if (section_index >= sections.Length)
        {
            // END LEVEL HERE
            section_index = 0;
        }

        // NOTE: This is just for the sake of having background playing after a
        // no-background section.
        if (sections[section_index].background_audio)
            conductor.playBackground = true;

        if(sections[section_index].l2_background_audio == 1)
            conductor.playL2BG1 = true;
        if(sections[section_index].l2_background_audio == 2)
            conductor.playL2BG2 = true;
        if(sections[section_index].l2_background_audio == 3)
            conductor.playL2BG3 = true;

        state = InterfaceState.DIALOGUE;
        dialogue.Enable();
        midiReader.Initialize(sections[section_index].midi_path);

        cam.isMoving = false;
    }

    public void TutorialStart()
    {   
        state = InterfaceState.TUTORIAL;
        tutorial.Enable();
        dialogue.Disable();
    }

    public void GameplayStart()
    {
        color.a = 0.0f;
        image.color = color;
        healthBar.showHealthBar = true;
        scoreManager.showScoreBar = true;
        comboManager.showComboBar = true;
        state = InterfaceState.GAMEPLAY;

        conductor.Reset();
        conductor.beats_till_first_note =
            sections[section_index].beats_till_first_note;
        switch (sections[section_index].audio_to_play)
        {
            case GameplayAudio.DRUMS:
                {
                    conductor.playDrumsNextBar = true;
                }
                break;
            case GameplayAudio.PIANO:
                {
                    conductor.playPianoNextBar = true;
                }
                break;
            case GameplayAudio.GUITAR:
                {
                    conductor.playGuitarNextBar = true;
                }
                break;
            case GameplayAudio.S1:
                {
                    conductor.playL2Section1 = true;
                }
                break;
            case GameplayAudio.S2:
                {
                    conductor.playL2Section2 = true;
                }
                break;
            case GameplayAudio.END:
                {
                    conductor.playL2End = true;
                }
                break;
        }
        if (sections[section_index].background_audio)
            conductor.playBackground = true;
        else
            conductor.playBackground = false;

        if(sections[section_index].l2_background_audio == 1)
            conductor.playL2BG1 = true;

        if(sections[section_index].l2_background_audio == 2)
            conductor.playL2BG2 = true;

        if(sections[section_index].l2_background_audio == 3)
            conductor.playL2BG3 = true;

        noteTrigger.Reset();
        tutorial.Disable();
        cam.isMoving = true;

        ++section_index;
    }
}
