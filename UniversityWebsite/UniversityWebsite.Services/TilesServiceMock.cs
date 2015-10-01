using System;
using System.Collections.Generic;
using UniversityWebsite.Services.Models;

namespace UniversityWebsite.Services
{
    public interface ITilesService
    {
        IEnumerable<Tile> GetTiles();
    }
    public class TilesServiceMock : ITilesService
    {
        public IEnumerable<Tile> GetTiles()
        {
            return new List<Tile>
            {
                new Tile{
                    Date = new DateTime(2015,12,12),
                    Header = "to jest header 1",
                    Href = "sfsadfsfsa",
                    Paragraph = "1 października w FN rozpoczyna się jeden z najstarszych i&nbsp;najbardziej prestiżowych konkursów muzycznych"
                },
                new Tile{
                    Date = new DateTime(2015,12,12),
                    Header = "to jest header 2",
                    Href = "sfsadfsfsa",
                    Paragraph = "1 października w FN rozpoczyna się jeden z najstarszych i&nbsp;najbardziej prestiżowych konkursów muzycznych"
                },
                new Tile{
                    Date = new DateTime(2015,12,12),
                    Header = "to jest header 3",
                    Href = "sfsadfsfsa",
                    Paragraph = "1 października w FN rozpoczyna się jeden z najstarszych i&nbsp;najbardziej prestiżowych konkursów muzycznych"
                },
            };

        }
    }
}
