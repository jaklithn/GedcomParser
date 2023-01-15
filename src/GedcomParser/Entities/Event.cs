using System.Collections.Generic;

namespace GedcomParser.Entities
{
    public class Event
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Type { get; set; }
        public DatePlace DatePlace { get; set; }
        public string Description { get; set; }

        public enum GedcomEventType
        {
            Unknown,

            Event,              // EVENT (EVEN) - Abstract event.

            Annulment,          // ANUL (ANNULMENT) - Declaring a marriage void from the beginning (retroactively invalid).
            Divorce,            // DIVORCE (DIV) - The legal dissolution of a marriage. 
            DivorceFiled,       // DIVORCE_FILED (DIVF) - An event of filing for a divorce by a spouse.
            Engagement,         // ENGAGEMENT (ENGA) - An event of recording or announcing an agreement between two people to become married.
            Marriage,           // MARRIAGE (MARR) - Marriage is an official and legal event, defined by the applicable law and customs of the land and the time, that creates a couple, possibly with children. This includes so-called common law marriages.
            MarriageBann,       // MARRIAGE_BANN (MARB) - An event of an official public notice given that two people intend to marry.
            MarriageContract,   // MARR_CONTRACT (MARC) - An event of recording a formal agreement of marriage, including the prenuptial agreement in which marriage partners reach agreement about the property rights of one or both, securing property to their children.
            MarriageLicense,    // MARR_LICENSE (MARL) - An event of obtaining a legal license to marry.
            MarriageSettlement, // MARR_SETTLEMENT (MARS) - An event of creating an agreement between two people contemplating marriage, at which time they agree to release or modify property rights that would otherwise arise from the marriage.

            Adoption,           // ADOPTION (ADOP) -  Adoption is a legal event that changes a child's legal parents from one set of parents to another set of parents. While some of the parents involved are likely to be biological or official parents, neither assumption should be made.
            Birth,              // BIRTH (BIRT) - The emergence of offspring from their mother as a separate being. Birth does not imply life. Birth includes stillbirth.
            Baptism,            // BAPTISM (BAPM) - The event of baptism, performed in infancy or later.
            BarMitzvah,         // BAR_MITZVAH (BARM) - The religious ceremony held when a Jewish boy reaches age 13.
            BasMitzvah,         // BAS_MITZVAH (BASM) - The religious ceremony held when a Jewish girl reaches age 13, also known as "Bat Mitzvah".
            Burial,             // BURIAL (BURI) - The action of burying a body.
            Census,             // CENSUS (CENS) - The event of the periodic count of the population for a designated locality, such as a national or state Census.
            Christening,        // CHRISTENING (CHR) - The religious event of baptising and naming a child.
            ChristeningAdult,   // ADULT_CHRISTENING (CHRA) - The religious event of baptizing and naming an adult person.
            Confirmation,       // CONFIRMATION (CONF) - The religious rite that confirms membership of a church (confirms because previously established by baptism).
            Cremation,          // CREMATION (CREM) - Disposal of a body by fire, by burning it to ashes.
            Death,              // DEATH (DEAT) - The end of a life.
            Emigration,         // EMIGRATION (EMIG) - An event of leaving one's homeland with the intent of residing elsewhere.
            FirstCommunion,     // FIRST_COMMUNION (FCOM) - Literally the first communion an individual partakes in. Communion is a rite within christian churches, and the first communion is considered a rite of passage.
            Graduation,         // GRADUATION (GRAD) - An event of awarding educational diplomas or degrees to individuals.
            Immigration,        // IMMIGRATION (IMMI) - An event of entering into a new locality with the intent of residing there.
            Naturalisation,     // NATURALISATION (NATU) - The event of obtaining citizenship.
            Retirement,         // RETIREMENT (RETI) - The event of ending one's occupational career.
            Probate,            // PROBATE (PROB) - An event of judicial determination of the validity of a will. May indicate several related court activities over several dates.
            Will,               // WILL (WILL) - A legal document treated as an event, by which a person disposes of his or her estate, to take effect after death. The event date is the date the will was signed while the person was alive.
        }
    }
}