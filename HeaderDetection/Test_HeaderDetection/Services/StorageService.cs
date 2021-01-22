using HeaderDetection.Interfaces;
using HeaderDetection.Models;

namespace Test_HeaderDetection.Services
{
    public class StorageService : IStorage
    {
        private readonly string[][] _headerStrings;

        public StorageService(string[][] headerStrings)
        {
            _headerStrings = headerStrings;
        }

        public void InsertText(string text, int rowZeroBase, int columnZeroBase)
        {
            _headerStrings[rowZeroBase][columnZeroBase] = text;
        }

        public void Insert(Item item, int rowZeroBase, int columnZeroBase)
        {
            _headerStrings[rowZeroBase][columnZeroBase] = item.Value?.ToString();
        }

        public void MergeRow(int rowZeroBase, int beginColumnZeroBase, int endColumnZeroBase)
        {
            for (var i = beginColumnZeroBase + 1; i <= endColumnZeroBase; i++)
            {
                _headerStrings[rowZeroBase][i] = "mergeRow";
            }
        }

        public void MergeColumn(int columnZeroBase, int beginRowZeroBase, int endRowZeroBase)
        {
            for (var i = beginRowZeroBase + 1; i <= endRowZeroBase; i++)
            {
                _headerStrings[i][columnZeroBase] = "mergeColumn";
            }
        }
    }
}