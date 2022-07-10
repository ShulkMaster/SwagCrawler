using System.Collections;

namespace SwagCrawler.Analysis;

public class DependencyGraph: IEnumerable<GraphNode>
{
    private readonly Dictionary<string, GraphNode> _nodes;

    public DependencyGraph(int gSize)
    {
        _nodes = new Dictionary<string, GraphNode>(gSize);
    }

    public bool Insert(GraphNode node)
    {
        return _nodes.TryAdd(node.RefIf, node);
    }

    public IEnumerator<GraphNode> GetEnumerator()
    {
        return new GraphEnum(_nodes.GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private sealed class GraphEnum : IEnumerator<GraphNode>
    {
        private readonly Dictionary<string, GraphNode>.Enumerator _origin;
        private Dictionary<string, GraphNode>.Enumerator _enumerator;

        public GraphEnum(Dictionary<string, GraphNode>.Enumerator dic)
        {
            _origin = dic;
            _enumerator = dic;
        }

        ~GraphEnum()
        {
            _origin.Dispose();
            _enumerator.Dispose();
        }
        
        public bool MoveNext()
        {
           return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator = _origin;
        }

        public GraphNode Current => _enumerator.Current.Value;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
    
}