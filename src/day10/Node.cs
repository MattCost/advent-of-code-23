public class Node
{
    public int Col { get; init; }
    public int Row { get; init; }
    public bool North { get; }
    public bool South { get; }
    public bool East { get; }
    public bool West { get; }

    public Node(int col, int row, char c)
    {
        Col=col;
        Row=row;
        switch(c)
        {
            case '|':
                North = true;
                South = true;
                break;
            case '-':
                East=true;
                West=true;
                break;
            case 'L':
                North = true;
                East = true;
                break;
            case 'J':
                North = true;
                West = true;
                break;
            case '7':
                South = true;
                West = true;
                break;
            case 'F':
                South = true;
                East = true;
                break;
            
            case '.':
            default:
                break;
        }
    }
    public (int,int,string) Traverse(string from)
    {
        switch(from)
        {
            case "North":
                if (!North) throw new Exception("north not true");
                if(East) return (Col+1, Row, "West");
                if(West) return (Col-1, Row, "East");
                if(South) return (Col,Row+1, "North");
                break;
                
            case "South":
                if (!South) throw new Exception("South not true");
                if(East) return (Col+1, Row, "West");
                if(West) return (Col-1, Row, "East");
                if(North) return (Col,Row-1,"South");
                break;

            case "East":
                if (!East) throw new Exception("east not true");
                if(West) return (Col-1, Row, "East");
                if(North) return (Col,Row-1,"South");
                if(South) return (Col,Row+1, "North");
                break;

            case "West":
                if (!West) throw new Exception("West not true");
                if(East) return (Col+1, Row, "West");
                if(North) return (Col,Row-1,"South");
                if(South) return (Col,Row+1, "North");
                break;
        }
        throw new Exception("lost");
    }
}