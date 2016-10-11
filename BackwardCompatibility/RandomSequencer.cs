using System;
using System.Collections;
using System.Diagnostics.Contracts;

namespace BackwardCompatibility
{
    /// <summary>
    /// Summary description for RandomSelector.
    /// </summary>
    public class RandomSequencer
    {
        public RandomSequencer()
        {
            sampler = new Random();
            items = new ArrayList();
            justDrawn = -1;
            Idx0 = Idx1 = 0;
        }

        public int Count
        {
            get { return items.Count - (Idx1 - Idx0); }
        }

        public void Add(object item, bool will_be_next)
        {
            if (Count == 0)
            {
                items.Add(item);
            }
            else
            {
                if (will_be_next)
                {
                    items.Add(items[Idx1]);
                    items[Idx1] = item;
                }
                else
                {
                    int pos = sampler.Next(Count);
                    if (pos < Idx0)
                    {
                        items.Add(items[Idx1]);
                        items[Idx1++] = null;
                        items[Idx0++] = items[pos];
                        items[pos] = item;
                    }
                    else
                    {
                        if (pos < Count)
                        {
                            items.Add(items[pos + Idx1 - Idx0]);
                            items[pos + Idx1 - Idx0] = item;
                        }
                        else
                        {
                            items.Add(item);
                        }
                    }
                }
            }

            justDrawn = -1;
        }

        public object Draw()
        {
            Contract.Requires(Count != 0, "There is nothing to draw");
            justDrawn = sampler.Next(Idx0);
            Swap(Idx0, Idx1++);
            Swap(Idx0++, justDrawn);
            RestoreIntegrity();
            return (items[justDrawn]);
        }

        public void DeleteLast()
        {
            Contract.Requires(justDrawn >= 0, "Deleting the last drawn is impossible");
            items[justDrawn] = null;
            if (justDrawn < Idx0)
            {
                Swap(justDrawn, --Idx0);
            }
            else
            {
                Swap(justDrawn, Idx1++);
                RestoreIntegrity();
            }

            justDrawn = -1;
        }

        protected void Swap(int index1, int index2)
        {
            object item = items[index1];
            items[index1] = items[index2];
            items[index2] = item;
        }

        protected void RestoreIntegrity()
        {
            if (Idx1 >= items.Count)
            {
                if (Idx1 > Idx0)
                {
                    items.RemoveRange(Idx0, Idx1 - Idx0);
                }

                Idx0 = Idx1 = 0;
            }
        }

        private Random sampler;
        private ArrayList items;
        private int justDrawn;
        private int Idx0; // [0..Idx0-1] - obiekty tasowane 
        private int Idx1; // [Idx1..] - obiekty potasowane, losowany jest zawsze Items[Idx1] 
    }
}
