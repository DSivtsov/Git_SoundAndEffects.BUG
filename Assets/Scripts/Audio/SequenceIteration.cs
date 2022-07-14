using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GMTools
{
    public enum SequenceType
    {
        FullyRND,
        NoRepeatRND,
        InSequence

    }
    public interface ISequenceIteration
    {
        int Next();
    }
    /// <summary>
    /// Abstract class for deriving
    /// </summary>
    public abstract class SequenceIterationAbstract : ISequenceIteration
    {
        protected int sequenceSize;

        public SequenceIterationAbstract(int size)
        {
            this.sequenceSize = size;
        }

        public abstract int Next();
    }

    public static class SequenceIterationManager
    {
        /// <summary>
        /// Get the reference to thr sequence of demnaded size and type
        /// </summary>
        /// <param name="sizeSequence"></param>
        /// <param name="sequenceType"></param>
        /// <param name="initialValue">Use Initial value in other cases set <see langword="null"/></param>
        /// <returns></returns>
        public static ISequenceIteration CreateSequenceIteration(int sizeSequence, SequenceType sequenceType, int? initialValue)
        {
            switch (sequenceType)
            {
                case SequenceType.FullyRND:
                    return new SequenceIterationRND(sizeSequence, initialValue);
                case SequenceType.NoRepeatRND:
                    return new SequenceIterationNoRepeatRND(sizeSequence, initialValue);
                case SequenceType.InSequence:
                    return new SequenceIterationInSequence(sizeSequence, initialValue);
                default:
                    Debug.LogError($"Error Init absent the [{sequenceType}] sequence type");
                    throw new NotImplementedException($"Error Init absent the [{sequenceType}] sequence type");
            }
        }
    }

    public class SequenceIterationRND : SequenceIterationAbstract
    {
        private System.Random rndIdxClip = new System.Random();
        /// <summary>
        /// Init the RND iteration
        /// </summary>
        /// <param name="size">size the sequence</param>
        /// <param name="seed">Use <see langword="null"/> for Random seed</param>
        public SequenceIterationRND(int size, int? seed = null) : base(size)
        {
            if (seed != null)
                rndIdxClip = new System.Random(seed.Value);
            else
                rndIdxClip = new System.Random();
        }

        public override int Next() => rndIdxClip.Next(sequenceSize);
    }

    public class SequenceIterationNoRepeatRND : SequenceIterationAbstract
    {
        private int[] arrOfIdx;
        private int currentIdxOfIdx;

        /// <summary>
        /// Init the RND iteration without repeat
        /// </summary>
        /// <param name="size">size the sequence</param>
        /// <param name="seed">Use <see langword="null"/> for Random seed</param>
        public SequenceIterationNoRepeatRND(int size, int? seed = null) : base(size)
        {
            arrOfIdx = Utils.FillArrayByShuffledIndexes(sequenceSize, seed);
            currentIdxOfIdx = 0;
        }

        //Possibile repeating in case of the new sequence begin from the last index of previous sequence
        public override int Next()
        {
            if (currentIdxOfIdx == sequenceSize)
            {
                Utils.FillArrayByShuffledIndexes(arrOfIdx);
                currentIdxOfIdx = 0;
            }
            //Debug.Log($"arrOfIdx[{currentIdxOfIdx}] = {arrOfIdx[currentIdxOfIdx]}");
            return arrOfIdx[currentIdxOfIdx++];
        }
    }

    public class SequenceIterationInSequence : SequenceIterationAbstract
    {
        private int currentIdxOfIdx;
        /// <summary>
        /// Init the iteration in sequence
        /// </summary>
        /// <param name="size">size the sequence</param>
        /// <param name="startNumber">Use <see langword="null"/> for start from 0</param>
        public SequenceIterationInSequence(int size, int? startNumber = null) : base(size)
        {
            int startIdx = 0;
            if (startNumber != null)
            {
                startIdx = startNumber.Value;
                startIdx = (startIdx < 0 || startIdx > sequenceSize - 1) ? 0 : startIdx; 
            }
            currentIdxOfIdx = startIdx;
        }

        public override int Next()
        {
            if (currentIdxOfIdx == sequenceSize)
                currentIdxOfIdx = 0;
            //Debug.Log($"currentIdxOfIdx=[{currentIdxOfIdx}]");
            return currentIdxOfIdx++;
        }
    }
}
