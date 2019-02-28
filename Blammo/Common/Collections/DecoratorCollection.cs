using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Agent.Common.Collections
{
    /// <summary>
    /// Represents a read-only bindable collection of objects that act as decorators
    /// for objects in a source ObservableCollection.
    /// </summary>
    /// <typeparam name="S">The type of objects in the source ObservableCollection</typeparam>
    /// <typeparam name="D">The type of objects exposed by this ReadOnlyObservableCollection.</typeparam>
    public class DecoratorCollection<S, D> : ReadOnlyObservableCollection<D>, IDisposable
    {
        /// <param name="sourceCollection">The collection of objects to be wrapped with decorators.</param>
        /// <param name="decoratorFactory">A delegate that creates new instances of a decorator class give an item from the sourceCollection.</param>
        public static DecoratorCollection<S, D> GetNew(
            INotifyCollectionChanged sourceCollection,
            Func<S, D> decoratorFactory)
        {
            //We need to do this little factory pattern to be able to pass an
            //ObservableCollection to the base class ReadOnlyObservableCollection
            //constructor and still maintain a reference to it in DecoratorCollection. 
            ObservableCollection<D> internalDecoratorCollection = new ObservableCollection<D>();

            return new DecoratorCollection<S, D>(sourceCollection,
                                                 decoratorFactory,
                                                 internalDecoratorCollection);
        }

        private readonly INotifyCollectionChanged _sourceCollection;
        private readonly Func<S, D> _decoratorFactory;
        private readonly ObservableCollection<D> _internalDecoratorCollection;
        private readonly Dictionary<D, S> _decoratorSourceDictionary;

        private DecoratorCollection(INotifyCollectionChanged sourceCollection,
                                    Func<S, D> decoratorFactory,
                                    ObservableCollection<D> internalDecoratorCollection)
            : base(internalDecoratorCollection)
        {
            _decoratorFactory = decoratorFactory;

            _internalDecoratorCollection = internalDecoratorCollection;

            _sourceCollection = sourceCollection;

            _decoratorSourceDictionary = new Dictionary<D, S>();

            InitializeInternalCollection(_internalDecoratorCollection,
                                              _sourceCollection,
                                              _decoratorFactory,
                                              _decoratorSourceDictionary);


            _sourceCollection.CollectionChanged += SourceCollection_CollectionChanged;
        }


        ~DecoratorCollection()
        {
            Dispose();
        }

        ///<summary>
        ///Don't leak event subscribers.
        ///</summary>
        ///<filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_sourceCollection != null)
            {
                _sourceCollection.CollectionChanged -= SourceCollection_CollectionChanged;
            }

            foreach (D d in _internalDecoratorCollection)
            {
                DisposeDecorator(d);
            }

            GC.SuppressFinalize(this);
        }


        private void InitializeInternalCollection(
            ObservableCollection<D> internalDecoratorCollection,
            INotifyCollectionChanged sourceCollection,
            Func<S, D> decoratorFactory,
            Dictionary<D, S> decoratorSourceDictionary)
        {
            foreach (S sourceObject in (IEnumerable<S>) sourceCollection)
            {
                D decorator = decoratorFactory(sourceObject);

                internalDecoratorCollection.Add(decorator);

                decoratorSourceDictionary.Add(decorator, sourceObject);
            }
        }

        /// <summary>
        /// Keep internal collection of decorators synchronized with contents
        /// of source collection of decorated objects.
        /// </summary>
        private void SourceCollection_CollectionChanged(object sender,
                                                        NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Reset:

                    foreach (D decorator in _internalDecoratorCollection)
                    {
                        DisposeDecorator(decorator);
                    }

                    _internalDecoratorCollection.Clear();

                    InitializeInternalCollection(_internalDecoratorCollection,
                                                      _sourceCollection,
                                                      _decoratorFactory,
                                                      _decoratorSourceDictionary);
                    break;

                case NotifyCollectionChangedAction.Replace:

                    D oldDecorator = _internalDecoratorCollection[args.OldStartingIndex];

                    DisposeDecorator(oldDecorator);

                    _decoratorSourceDictionary.Remove(oldDecorator);

                    S newSource = (S) args.NewItems[0];

                    _internalDecoratorCollection[args.OldStartingIndex] =
                        _decoratorFactory(newSource);

                    _decoratorSourceDictionary.Add(
                        _internalDecoratorCollection[args.OldStartingIndex],
                        newSource);
                    break;

                case NotifyCollectionChangedAction.Remove:

                    oldDecorator = _internalDecoratorCollection[args.OldStartingIndex];

                    DisposeDecorator(oldDecorator);

                    _internalDecoratorCollection.RemoveAt(args.OldStartingIndex);

                    _decoratorSourceDictionary.Remove(oldDecorator);

                    break;
#if !SILVERLIGHT
                case NotifyCollectionChangedAction.Move:

                    _internalDecoratorCollection.Move(args.OldStartingIndex,
                                                      args.NewStartingIndex);
                    break;
#endif
                case NotifyCollectionChangedAction.Add:

                    int startIndex = args.NewStartingIndex;

                    foreach (S newSourceObject in args.NewItems)
                    {
                        D newDecorator = _decoratorFactory(newSourceObject);

                        _internalDecoratorCollection.Insert(startIndex, newDecorator);

                        startIndex++;

                        _decoratorSourceDictionary.Add(newDecorator, newSourceObject);
                    }
                    break;
            }
        }

        public S GetSource(D decorator)
        {
            return _decoratorSourceDictionary[decorator];
        }

        private void DisposeDecorator(D d)
        {
            if (d is IDisposable)
            {
                ((IDisposable) d).Dispose();
            }
        }
    }
}