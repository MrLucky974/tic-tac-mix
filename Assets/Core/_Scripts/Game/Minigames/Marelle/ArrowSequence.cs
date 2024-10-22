using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RapidPrototyping.TicTacMix
{
    public class ArrowSequence : MonoBehaviour
    {
        [Header("RandomArrows")]
        private KeyCode[] _allDirectionsArrows = { KeyCode.UpArrow, KeyCode.DownArrow,KeyCode.LeftArrow, KeyCode.RightArrow };
        private KeyCode[] _allDirectionsKeys = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D }; //Pour ZQSD
        [SerializeField] private int[] _sequenceNumber ; //A augmenter par séquence
        private List<KeyCode> _inputSequenceKey = new List<KeyCode>();
        public List<KeyCode> _inputSequenceArrow = new List<KeyCode>();
        private int[] _currentIndex = { 0, 0 };

        [Header("Instantiate")]

        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private Transform[] _placement;
        [SerializeField] private int _gapInBetween = 100;

        [SerializeField] private Sprite[] _arrowSprite; //Ref aux sprites des 4 flèches
        private List<GameObject> _instanciatedKey = new List<GameObject>();
        private List<GameObject> _instanciatedArrow = new List<GameObject>();


        private void Start()
        {

            RandomArrowsSequence(_allDirectionsKeys, _sequenceNumber[0], _inputSequenceKey, _placement[0], _instanciatedKey );
            RandomArrowsSequence(_allDirectionsArrows, _sequenceNumber[1], _inputSequenceArrow, _placement[1], _instanciatedArrow);
        }

        private void Update()
        {          

            //Si la touche cliquée est la même que la séquence; continuer la séquence
            if (Input.anyKeyDown)
            {
                KeyCode pressedKey = GetPressedKey(_allDirectionsKeys);

                if (pressedKey != KeyCode.None)
                {
                    if (pressedKey == _inputSequenceKey[_currentIndex[0]])
                    {
                        print(pressedKey + ":" + _inputSequenceKey[_currentIndex[0]]);
                        _currentIndex[0]++;
                        Destroy(_instanciatedKey[_currentIndex[0] - 1]);



                        // Vérifier si la séquence est complète
                        if (_currentIndex[0] >= _inputSequenceKey.Count)
                        {

                            _currentIndex[0] = 0;
                            _instanciatedKey.Clear();
                            _sequenceNumber[0]++;
                            RandomArrowsSequence(_allDirectionsKeys, _sequenceNumber[0], _inputSequenceKey, _placement[0], _instanciatedKey);

                        }
                    }
                    else
                    {
                        ResetSequence(_instanciatedKey, _inputSequenceKey);
                        print("wrong");
                        //Mauvaise touche : Retour à la case départ + Reset

                    }
                }
                KeyCode pressedArrow = GetPressedKey(_allDirectionsArrows);
                if (pressedArrow != KeyCode.None)
                {
                    if (pressedArrow == _inputSequenceArrow[_currentIndex[1]])
                    {
                        _currentIndex[1]++;
                        Destroy(_instanciatedArrow[_currentIndex[1] - 1]);



                        // Vérifier si la séquence est complète
                        if (_currentIndex[1] >= _inputSequenceArrow.Count)
                        {

                            _currentIndex[1] = 0;
                            _instanciatedArrow.Clear();
                            _sequenceNumber[1]++;
                            RandomArrowsSequence(_allDirectionsArrows, _sequenceNumber[1], _inputSequenceArrow, _placement[1], _instanciatedArrow);
                        }
                    }
                    else
                    {
                        ResetSequence(_instanciatedArrow, _inputSequenceArrow);
                        print("wrong");
                        //Mauvaise touche : Retour à la case départ + Reset

                    }
                }
            }

        }

        /*
            //NE FONCTIONNE PAS
            //if (Input.anyKeyDown)
            //{
            //    GetKeyDown(_allDirectionsKeys, _inputSequenceKey, _currentIndex[0], _instanciatedKey, _sequenceNumber[0], _placement[0]);
            //    GetKeyDown(_allDirectionsArrows, _inputSequenceArrow, _currentIndex[1], _instanciatedArrow, _sequenceNumber[1], _placement[1]);
            //}

        void GetKeyDown(KeyCode[] alldirections, List<KeyCode> inputsequence, int currentindex, List<GameObject> instanciated, int sequencenumber, Transform placement)
        {

                KeyCode pressedKey = GetPressedKey(alldirections);

                if (pressedKey != KeyCode.None)
                {
                    if (pressedKey == inputsequence[currentindex])
                    {
                        currentindex++;
                        Destroy(instanciated[currentindex - 1]);



                        // Vérifier si la séquence est complète
                        if (currentindex >= inputsequence.Count)
                        {

                            currentindex = 0;
                            instanciated.Clear();
                            sequencenumber++;
                            RandomArrowsSequence(alldirections, sequencenumber, inputsequence, placement, instanciated);

                        }
                    }
                    else
                    {
                        ResetSequence(instanciated, inputsequence);
                        RandomArrowsSequence(alldirections, sequencenumber, inputsequence, placement, instanciated);
                        print("wrong");
                        //Mauvaise touche : Retour à la case départ + Reset

                    }
                }
            
        }
        */


        //Parmi la liste des directions, en prendre le sequenceNumber ; et l'ajouter dans la liste avec les images correspondantes
        void RandomArrowsSequence(KeyCode[] input, int sequenceNumber, List<KeyCode> inputSequence, Transform placement, List<GameObject> instanciated)
        {

            inputSequence.Clear();

            float totalLength = (sequenceNumber - 1) * _gapInBetween;

            for (int i = 0; i < sequenceNumber; i++)
            {
                //Créer une séquence random
                KeyCode randomKey = input[Random.Range(0, input.Length)];
                inputSequence.Add(randomKey);


                //Instantiate la sequence
                GameObject arrows = Instantiate(_arrowPrefab, placement);

                RectTransform rectTransform = arrows.GetComponent<RectTransform>();


                //Mettre au milieu de placement
                float yOffset = i * _gapInBetween - totalLength / 2f;
                rectTransform.anchoredPosition = new Vector2(0, yOffset);

                //Ajout des prefabs pour supprimer 
                instanciated.Add(arrows);

                //Assigner le sprite du prefab au sprite de l'arrow
                Image arrowImage = arrows.GetComponent<Image>();
                arrowImage.sprite = GetSpriteForKey(randomKey);
               
            }

            //Debug.Log("New Sequence: " + string.Join(", ", _inputSequence));
        }

        //Reset
        void ResetSequence(List<GameObject> instanciated, List<KeyCode> inputSequence)
        {
     
            RandomArrowsSequence(_allDirectionsKeys, _sequenceNumber[0], _inputSequenceKey, _placement[0], _instanciatedKey);

            for (int i = 0; i < instanciated.Count; i++)
            {
                Destroy(instanciated[i]);
                instanciated.Clear();
                inputSequence.Clear();

            }          
        }

        //Prendre la touche qui est cliquée
        KeyCode GetPressedKey(KeyCode[] input)
        {
            foreach (KeyCode key in input)
            {
                if (Input.GetKeyDown(key))
                { return key; }
            }

            return KeyCode.None;

        }

        //Assigner un sprite à chaque KeyCode
        Sprite GetSpriteForKey(KeyCode key)
        {
            if (key == KeyCode.W)
            {
                return _arrowSprite[0];
            }
            else if (key == KeyCode.S)
            {
                return _arrowSprite[1];
            }
            else if (key == KeyCode.A)
            {
                return _arrowSprite[2];
            }
            else if (key == KeyCode.D)
            {
                return _arrowSprite[3];
            }
            if (key == KeyCode.UpArrow)
            {
                return _arrowSprite[4];
            }
            else if (key == KeyCode.DownArrow)
            {
                return _arrowSprite[5];
            }
            else if (key == KeyCode.LeftArrow)
            {
                return _arrowSprite[6];
            }
            else if (key == KeyCode.RightArrow)
            {
                return _arrowSprite[7];
            }
            return null;
        }

    }
}
