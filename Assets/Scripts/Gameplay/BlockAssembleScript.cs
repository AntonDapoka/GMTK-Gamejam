using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;
using TMPro;

public class BlockAssembleScript : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private BlockTypesHolderScript typesHolder;
    [SerializeField] private AssemblerSlotScript[] assemblerSlots;

    [SerializeField] private PlayerShootingScript PSS;
    [SerializeField] private TextMeshPro textPlayer;

    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

    [SerializeField] private int startBlockIndex;
    [SerializeField] private int endBlockIndex;
    [SerializeField] private float delay = 1f;
    [SerializeField] private bool[] disabled;

    private void Awake()
    {
        startBlockIndex = 0;
        SetBlock(0, startBlockIndex); //Set BlockStart
        endBlockIndex = assemblerSlots.Length - 1;
        SetBlock(1, endBlockIndex); //Set BlockEnd

        for (int i = 0; i < assemblerSlots.Length; i++)
        {
            assemblerSlots[i].index = i;
        }
    }

    private void Start()
    {
        StartCoroutine(MainLoop());
    }

    private IEnumerator MainLoop()
    {
        int index = 0;


        while (true)
        {
            int minIndex = startBlockIndex;
            int maxIndex = endBlockIndex + 1;

            if (maxIndex > minIndex)
            {
                for (int i = minIndex; i < maxIndex; i++)
                {
                    AssemblerSlotScript slot = assemblerSlots[i];
                    //slot.GetComponent<Image>().color = i % 2 == 0 ? color1 : color2;
                    //slot.GetComponent<Image>().color/ = 1f;
                    DraggableBlockScript blockInSlot = slot.GetComponentInChildren<DraggableBlockScript>();
                    if (blockInSlot != null)
                    {
                        ActivateBlock(blockInSlot, i);
                    }


                    yield return new WaitForSeconds(delay);
                }
                    //assemblerSlots[i].GetComponent<Image>().color = Color.random;
            }
            yield return new WaitForSeconds(delay);

            //Debug.Log("-----------------------------------------------");
            //index = (index + 1) % assemblerSlots.Length;
        }
    }

    private void SetBlock(int id, int pos)
    {
        BlockScript block = typesHolder.blocksTypes[id];
        SpawnBlock(block, assemblerSlots[pos]);
    }

    public void SpawnBlock(BlockScript block, AssemblerSlotScript slot)
    {
        GameObject newBlockGo = Instantiate(block.prefab, slot.transform);
        DraggableBlockScript draggableBlock = newBlockGo.GetComponent<DraggableBlockScript>();
        draggableBlock.InitializeBlock(block);
    }

    public void ChangeBlockStartIndex(int id)
    {
        startBlockIndex = id;
    }

    public void ChangeBlockEndIndex(int id)
    {
        endBlockIndex = id;
    }

    private void ActivateBlock(DraggableBlockScript block, int index)
    {
        BlockType type = block.block.typeBlock;

        if (type == BlockType.Write)// && disabled[index] != true)
        {
            textPlayer.text = block.GetComponentInChildren<WriteBlockScript>().GetText();
            textPlayer.gameObject.SetActive(true);
            StartCoroutine(WaitAndTurnOff(textPlayer.gameObject, delay));
        }

        if (type == BlockType.Shoot)// && disabled[index] != true)
        {
            PSS.ShootBullet();
        }
    }

    private IEnumerator WaitAndTurnOff(GameObject gameObject, float duration)
    {

        yield return new WaitForSeconds(duration*2);
        gameObject.SetActive(false);
    }
}
