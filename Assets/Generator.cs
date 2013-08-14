using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class NoteInfo
{
    public int note;
    public List<GameObject> elements;

    public NoteInfo (int note)
    {
        this.note = note;
        elements = new List<GameObject> ();
    }
}

public class Generator : MonoBehaviour
{
    public GameObject prefab;
    List<NoteInfo> noteInfoList;

    void Awake ()
    {
        noteInfoList = new List<NoteInfo> ();
    }

    void OnNoteOn (MidiMessage midi)
    {
        int note = midi.data1;
        int velocity = midi.data2;
        
        foreach (var ni in noteInfoList) {
            if (ni.note == note)
                return;
        }

        int numElements = Mathf.Max (8 * velocity / 128, 1);

        var noteInfo = new NoteInfo (note);
        for (var i = 0; i < numElements; i++) {
            var element = Instantiate (prefab, transform.position, Random.rotation) as GameObject;
            noteInfo.elements.Add (element);
            element.GetComponent<Element> ().ApplyMidiMessage (midi);
        }

        noteInfoList.Add (noteInfo);
    }

    void OnNoteOff (MidiMessage midi)
    {
        int note = midi.data1;

        NoteInfo niFound = null;
        foreach (var ni in noteInfoList) {
            if (ni.note == note) {
                niFound = ni;
                break;
            }
        }

        if (niFound != null) {
            foreach (var e in niFound.elements) {
                e.GetComponent<Element> ().StartShrink ();
            }
            noteInfoList.Remove (niFound);
        }
    }
}
