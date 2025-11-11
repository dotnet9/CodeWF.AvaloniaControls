using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CodeWF.AvaloniaControls.Extensions
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool _suppressNotification;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!_suppressNotification)
                base.OnCollectionChanged(e);
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Sort()
        {
            if (Items is List<T> items)
            {
                items.Sort();
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Sort(Comparer<T> comparer)
        {
            if (Items is List<T> items)
            {
                items.Sort(comparer);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void Sort(Comparison<T> comparer)
        {
            if (Items is List<T> items)
            {
                items.Sort(comparer);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            _suppressNotification = true;
            try
            {
                foreach (var item in collection)
                    base.Add(item);
            }
            finally
            {
                _suppressNotification = false;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            var items = collection.ToList();
            if (!items.Any())
                return;

            _suppressNotification = true;
            try
            {
                int offset = 0;
                foreach (var item in items)
                {
                    base.Insert(index + offset, item);
                    offset++;
                }
            }
            finally
            {
                _suppressNotification = false;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var itemsToRemove = collection.ToList();
            if (!itemsToRemove.Any())
                return;

            _suppressNotification = true;
            try
            {
                var indexes = new List<int>();
                foreach (var item in itemsToRemove)
                {
                    int index = IndexOf(item);
                    if (index >= 0 && !indexes.Contains(index))
                        indexes.Add(index);
                }

                foreach (int index in indexes.OrderByDescending(i => i))
                    base.RemoveAt(index);
            }
            finally
            {
                _suppressNotification = false;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(int index, int count)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0 || index + count > Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                return;

            _suppressNotification = true;
            try
            {
                // 从后往前删除，确保索引正确性
                for (int i = count - 1; i >= 0; i--)
                    base.RemoveAt(index + i);
            }
            finally
            {
                _suppressNotification = false;
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}