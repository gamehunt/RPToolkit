using Exiled.API.Features;
using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace RPToolkit
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public Util.CustomNameType CustomNameType { get; set; }

        public Dictionary<RoleType, string> CustomNameFormat { get; set; } = new Dictionary<RoleType, string>()
        {
            {RoleType.ClassD, "D-%d%%d%%d%%d%%d%"},
            {RoleType.Scientist, "Mr. %lu%.%random_surname%"},
            {RoleType.FacilityGuard, "%random_name% %random_surname%"},
            {RoleType.NtfCadet, "NTF Cadet  %lu%.%random_surname%"},
            {RoleType.NtfLieutenant, "NTF Lieutenant  %lu%.%random_surname%"},
            {RoleType.NtfCommander, "NTF Commander  %lu%.%random_surname%" },
            {RoleType.NtfScientist, "NTF Support Unit  %lu%.%random_surname%" },
            {RoleType.ChaosInsurgency, "CI Agent %random_name%"},
        };

        public string CustomNameBroadcast { get; set; } = "Your name: %name%";

        public Dictionary<RoleType, List<PlayerInfoArea>> HiddenPlayerInfoElements { get; set; } = new Dictionary<RoleType, List<PlayerInfoArea>>();

        public bool RandomizePlayerSizes { get; set; } = true;
        public float[] RandomSizePlayerSizeMinimum { get; set; } = new float[] { 0.8f, 0.8f, 0.8f };
        public float[] RandomSizePlayerSizeMaximum { get; set; } = new float[] { 1.1f, 1.1f, 1.1f };
        public RoleType[] RandomSizeIgnoreRoles { get; set; } = new RoleType[] { RoleType.Spectator, RoleType.Scp079, RoleType.None };

        public static readonly string Names =
@"James
John
Robert
Michael
William
David
Richard
Joseph
Thomas
Charles
Christopher
Daniel
Matthew
Anthony
Donald
Mark
Paul
Steven
Andrew
Kenneth
Joshua
Kevin
Brian
George
Edward
Ronald
Timothy
Jason
Jeffrey
Ryan
Jacob
Gary
Nicholas
Eric
Jonathan
Stephen
Larry
Justin
Scott
Brandon
Benjamin
Samuel
Frank
Gregory
Raymond
Alexander
Patrick
Jack
Dennis
Jerry
Tyler
Aaron
Jose
Henry
Adam
Douglas
Nathan
Peter
Zachary
Kyle
Walter
Harold
Jeremy
Ethan
Carl
Keith
Roger
Gerald
Christian
Terry
Sean
Arthur
Austin
Noah
Lawrence
Jesse
Joe
Bryan
Billy
Jordan
Albert
Dylan
Bruce
Willie
Gabriel
Alan
Juan
Logan
Wayne
Roy
Eugene
Randy
Vincent
Russell
Louis
Philip
Bobby
Johnny
Bradley
";

        public static readonly string Surnames =
@"Smith
Jones
Williams
Taylor
Brown
Davies
Evans
Wilson
Thomas
Johnson
Roberts
Robinson
Thompson
Wright
Walker
White
Edwards
Hughes
Green
Hall
Lewis
Harris
Clarke
Patel
Jackson
Wood
Turner
Martin
Cooper
Hill
Ward
Morris
Moore
Clark
Lee
King
Baker
Harrison
Morgan
Allen
James
Scott
Phillips
Watson
Davis
Parker
Price
Bennett
Young
Griffiths
Mitchell
Kelly
Cook
Carter
Richardson
Bailey
Collins
Bell
Shaw
Murphy
Miller
Cox
Richards
Khan
Marshall
Anderson
Simpson
Ellis
Adams
Singh
Begum
Wilkinson
Foster
Chapman
Powell
Webb
Rogers
Gray
Mason
Ali
Hunt
Hussain
Campbell
Matthews
Owen
Palmer
Holmes
Mills
Barnes
Knight
Lloyd
Butler
Russell
Barker
Fisher
Stevens
Jenkins
Murray
Dixon
Harvey
";

        public static string RPToolkitRootPath = Path.Combine(Paths.Configs, "rptoolkit");
        public static string NamesPath = Path.Combine(RPToolkitRootPath, "names.txt");
        public static string SurnamesPath = Path.Combine(RPToolkitRootPath, "surnames.txt");

        

        public Dictionary<string, ItemType[]> ItemGroups { get; set; } = new Dictionary<string, ItemType[]>()
        {
            {"HeavyWeapons", new ItemType[]{ItemType.GunE11SR, ItemType.GunLogicer, ItemType.MicroHID}}
        };
        public Dictionary<RoleType, Dictionary<string, int>> ItemGroupsRestrictions { get; set; } = new Dictionary<RoleType, Dictionary<string, int>>()
        {
            {RoleType.ClassD, new Dictionary<string, int>()
                {
                    { "HeavyWeapons", 1}
                }
            }
        };
        public string CustomGroupLimitReachedHint { get; set; } = "You can't carry more items of this type";
        public Dictionary<RoleType, int> InventoryRestrictions { get; set; } = new Dictionary<RoleType, int>() {
            {RoleType.ClassD, 4}
        };
        public string CustomInventoryLimitReachedHint { get; set; } = "You can't carry more items!";

        //Only for human classes
        public Dictionary<RoleType, float> WalkSpeedMultipliers { get; set; } = new Dictionary<RoleType, float>();
        public Dictionary<RoleType, float> SprintSpeedMultipliers { get; set; } = new Dictionary<RoleType, float>()
        {
            {RoleType.Scientist, 0.8f}
        };

        public bool ApplyOnlyIfItemInHand { get; set; } = true;

        public Dictionary<RoleType, Dictionary<ItemType, float>> ItemsWalkSpeedMultipliers { get; set; } = new Dictionary<RoleType, Dictionary<ItemType, float>>()
        {
            {
                RoleType.ClassD, new Dictionary<ItemType, float>()
                {
                    {ItemType.MicroHID, 0.5f}
                } 
            }
        };

        public Dictionary<RoleType, Dictionary<ItemType, float>> ItemsSprintSpeedMultipliers { get; set; } = new Dictionary<RoleType, Dictionary<ItemType, float>>()
        {
            {
                RoleType.ClassD, new Dictionary<ItemType, float>()
                {
                    {ItemType.MicroHID, 0.5f}
                }
            }
        };

        public float PunchRange { get; set; } = 5f;
        public float PunchCooldown { get; set; } = 3f;

        public Dictionary<RoleType, float> PunchDamage { get; set; } = new Dictionary<RoleType, float>()
        {
            { RoleType.ClassD, 5f },
            { RoleType.Scientist, 5f },
            { RoleType.ChaosInsurgency, 15f },
            { RoleType.FacilityGuard, 10f },
            { RoleType.NtfCadet, 10f },
            { RoleType.NtfLieutenant, 15f },
            { RoleType.NtfScientist, 15f },
            { RoleType.NtfCommander, 15f }
        };

        public float StealRange { get; set; } = 2f;
        public float StealCooldown { get; set; } = 30f;
        public Dictionary<RoleType, float> StealChance { get; set; } = new Dictionary<RoleType, float>()
        {
            { RoleType.ClassD, 5f },
            { RoleType.Scientist, 5f },
        };

        public string StealerBroadcastSuccess { get; set; } = "You successfully stole things of %name%";
        public string StealerBroadcastFail { get; set; } = "Failed to steal things!";
        public string StealVictimBroadcastSuccess { get; set; } = "Somebody stole your things!";
        public string StealVictimBroadcastFail { get; set; } = "%name% tried to steal your things!";
    }
}