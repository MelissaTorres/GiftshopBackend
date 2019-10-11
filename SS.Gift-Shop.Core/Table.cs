using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SS.GiftShop.Core
{
    public class Table
    {
        private readonly List<Row> _rows;
        public IList<string> Columns { get; }

        public IReadOnlyList<Row> Rows
        {
            get { return _rows; }
        }

        public Table(IEnumerable<string> columns)
        {
            if (columns is null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            Columns = new List<string>(columns);
            _rows = new List<Row>();
        }

        public void AddRow(IEnumerable<object> values)
        {
            var row = new Row(this, values);
            _rows.Add(row);
        }

        [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
        [DebuggerTypeProxy(typeof(RowDebugView))]
        public class Row
        {
            private readonly object[] _items;

            internal Table Table { get; }

            public int Count
            {
                get { return _items.Length; }
            }

            public IReadOnlyList<object> Values
            {
                get { return _items; }
            }

            public object this[string key]
            {
                get
                {
                    if (key == null)
                    {
                        throw new ArgumentNullException(nameof(key));
                    }

                    var index = Table.Columns.IndexOf(key);
                    if (index < 0)
                    {
                        throw new ArgumentException(key);
                    }

                    return _items[index];
                }
            }

            public object this[int index]
            {
                get
                {
                    if (index < 0 || index >= Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }
                    return _items[index];
                }
            }

            internal Row(Table table, IEnumerable<object> values)
            {
                Table = table;
                _items = new object[table.Columns.Count];

                int index = 0;
                foreach (var value in values)
                {
                    _items[index] = value;
                    index++;
                }
            }
        }

        internal sealed class RowDebugView
        {
            private readonly Row _row;

            /// <summary>
            /// Initializes a new instance of the <see cref="RowDebugView"/> class.
            /// </summary>
            /// <param name="row">The row.</param>
            public RowDebugView(Row row)
            {
                _row = row;
            }

            /// <summary>
            /// Gets the items.
            /// </summary>
            /// <value>The items.</value>
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public KeyValuePair[] Items
            {
                get
                {
                    return _row.Table.Columns
                        .Zip(_row.Values, (key, value) => new KeyValuePair(key, value))
                        .ToArray();
                }
            }

            /// <summary>
            /// Represent a single key/value pair for the debugger
            /// </summary>
            [DebuggerDisplay("{" + nameof(Value) + "}", Name = "[{" + nameof(Key) + ",nq}]", Type = "")]
            internal sealed class KeyValuePair
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="KeyValuePair"/> class.
                /// </summary>
                /// <param name="key">The key.</param>
                /// <param name="value">The value.</param>
                public KeyValuePair(string key, object value)
                {
                    Key = key;
                    Value = value;
                }

                public string Key { get; }

                public object Value { get; }
            }
        }
    }
}
