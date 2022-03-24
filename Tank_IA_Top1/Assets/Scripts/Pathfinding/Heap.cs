using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public int Count
    {
        get { return currentItemCount; }
    }

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirstItem()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].heapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    public bool Contains(T item)
    {
        return Equals(items[item.heapIndex], item);
    }

    private void SortDown(T item)
    {
        /* Left child index = 2*itemnIndex + 1
         * right child index = 2*itemnIndex + 2 
         */
        while (true)
        {
            int childIndexLeft = item.heapIndex * 2 + 1;
            int childIndexRigth = item.heapIndex * 2 + 2;

            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRigth < currentItemCount)
                {
                    if(items[childIndexLeft].CompareTo(items[childIndexRigth]) < 0)
                    {
                        swapIndex = childIndexRigth;
                    }
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    SwapItems(item, items[swapIndex]);
                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }
        }
    }

    private void SortUp(T item)
    {
        /* Parent index = (itemIndex-1)/2
         */
        int parentIndex = (item.heapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            //If item have a lower priority (in our case a lower fCost)
            if(item.CompareTo(parentItem) > 0)
            {
                SwapItems(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.heapIndex - 1) / 2;
        }
    }

    private void SwapItems(T itemA, T itemB)
    {
        //Swapping their places in the array
        items[itemA.heapIndex] = itemB;
        items[itemB.heapIndex] = itemA;

        //Swapping the indexes
        int itemAIndex = itemA.heapIndex;
        itemA.heapIndex = itemB.heapIndex;
        itemB.heapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int heapIndex { get; set; }
}
