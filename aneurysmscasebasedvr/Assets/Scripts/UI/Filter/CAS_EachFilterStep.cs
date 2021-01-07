using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CAS
{
    public class CAS_EachFilterStep : MonoBehaviour
    {
        //Filters added display
        public Transform parentContent;
        public GameObject prefab;

        Dictionary<string, string> filterForThisStep;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ClickFilterButton()
        {

        }

        public void ClickGroupButton()
        {

        }

        public void SetFiltersForThisStep(Dictionary<string, string> filters)
        {
            filterForThisStep = filters;

            foreach (string filter in filters.Keys)
            {
                GameObject fitlerCotentPrefab = Instantiate(prefab);
                prefab.GetComponent<CAS_EachFilterStepFilters>().SetContent(filters);
            }
        }
    }
}

