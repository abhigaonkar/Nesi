export class FilterRow {
    public column: string;
    public key: string;
    public operand: {
        label: string,
        value: string
    };
    public placeholder: string;
    public value: any = '';
    public type: string;
    public showInputControl = false;
    public showPlaceholder: boolean;
    public useAliasing = false;
    public rowCreationSource: string;
    public reservedKey: string;
    public reservedOperand: string;
    public alias?: string;
    constructor() {
        this.value = '';
        this.showPlaceholder = true;
    }
}


export class FilterRoot {
    public filterOperator: string;
    public filterRows: Array<FilterRow> = [];
    public children: FilterRoot[] = [];
    public complexDateTimeList: Array<ComplexDateTimeObject> = [];
    public parentFilterRows: Array<FilterRow> = [];
    public level: number;
    public isComplexDateTime = false;
    public isDirty = false;
    public alias?: string;
    public columnName?: string;

    constructor() { }

}

export class ComplexDateTimeObject {
    columnName: string;
    condition1: LabelValue;
    operand: string;
    condition2: LabelValue;
    dateType: string;
    alias: string;

    constructor() {
        this.dateType = 'complexDateTime';
    }
}

export class LabelValue {

    constructor(public label: string,
        public value: string, public displayValue?: string, public type?: string) { }
}

export let FILTER_OPERATORS = [
    {
        'imgSrc': '',
        'label': 'And'
    },
    {
        'imgSrc': '',
        'label': 'Or'
    },
    {
        'imgSrc': '',
        'label': 'Not And'
    },
    {
        'imgSrc': '',
        'label': 'Not Or'
    },
    {
        'imgSrc': '',
        'label': 'Add Condition'
    }
]
//

export let COMPLEXDATE_CONSTANT = {
    GREATERTHANOREQUAL_LOCALDATETIMEYESTERDAY: '^greaterThanOrEqual^LocalDateTimeYesterday^',
    LESSTHAN_LOCALDATETIMETODAY: '^lessThan^LocalDateTimeToday^',
    GREATERTHANOREQUAL_LOCALDATETIMETODAY: '^greaterThanOrEqual^LocalDateTimeToday^',
    LESSTHAN_LOCALDATETIMETOMORROW: '^lessThan^LocalDateTimeTomorrow^',
    GREATERTHANOREQUAL_LOCALDATETIMETOMORROW: '^greaterThanOrEqual^LocalDateTimeTomorrow^',
    LESSTHAN_LOCALDATETIMEDAYAFTERTOMORROW: '^lessThan^LocalDateTimeDayAfterTomorrow^',
    GREATERTHANOREQUAL_LOCALDATETIMELASTWEEK: '^greaterThanOrEqual^LocalDateTimeLastWeek^',
    LESSTHAN_LOCALDATETIMETHISWEEK: '^lessThan^LocalDateTimeThisWeek^',
    GREATERTHANOREQUAL_LOCALDATETIMETHISWEEK: '^greaterThanOrEqual^LocalDateTimeThisWeek^',
    LESSTHAN_LOCALDATETIMENEXTWEEK: '^lessThan^LocalDateTimeNextWeek^',
    GREATERTHANOREQUAL_LOCALDATETIMENEXTWEEK: '^greaterThanOrEqual^LocalDateTimeNextWeek^',
    LESSTHAN_LOCALDATETIMETWOWEEKSAWAY: '^lessThan^LocalDateTimeTwoWeeksAway^',
    GREATERTHANOREQUAL_LOCALDATETIMENEXTYEAR: '^greaterThanOrEqual^LocalDateTimeNextYear^',
    LESSTHAN_NEXTYEAR: '^lessThan^AddYears(LocalDateTimeNextYear,1)^',
    GREATERTHANOREQUAL_LOCALDATETIMETHISYEAR: '^greaterThanOrEqual^LocalDateTimeThisYear^',
    LESSTHAN_LOCALDATETIMENEXTYEAR: '^lessThan^LocalDateTimeNextYear^',
    GREATERTHANOREQUAL_LASTYEAR: '^greaterThanOrEqual^AddYears(LocalDateTimeThisYear,-1)^',
    LESSTHAN_LOCALDATETIMETHISYEAR: '^lessThan^LocalDateTimeThisYear^',
    GREATETHANOREQUAL_LOCALDATETIMENEXTMONTH: '^greaterThanOrEqual^LocalDateTimeNextMonth^',
    LESSTHAN_NEXTMONTH: '^lessThan^AddMonths(LocalDateTimeNextMonth,1)^',
    GREATERTHANOREQUAL_LOCALDATETIMETHISMONTH: '^greaterThanOrEqual^LocalDateTimeThisMonth^',
    LESSTHAN_LOCALDATETIMENEXTMONTH: '^lessThan^LocalDateTimeNextMonth^',
    GREATERTHANOREQUAL_LASTMONTH: '^greaterThanOrEqual^AddMonths(LocalDateTimeThisMonth,-1)^',
    LESSTHAN_LOCALDATETIMETHISMONTH: '^lessThan^LocalDateTimeThisMonth^',
    FROMDATE_GREATERTHANOREQUAL: 'greaterThanOrEqual',
    TODATE_LESSTHAN: 'lessThan'
}

export let COMPLEX_DATE_CONDITIONS: Array<LabelValue> = [
    { label: 'LocalDateTimeYesterday', value: 'LocalDateTimeYesterday' },
    { label: 'LocalDateTimeToday', value: 'LocalDateTimeToday' },
    { label: 'LocalDateTimeTomorrow', value: 'LocalDateTimeTomorrow' },
    { label: 'LocalDateTimeDayAfterTomorrow', value: 'LocalDateTimeDayAfterTomorrow' },
    { label: 'LocalDateTimeLastWeek', value: 'LocalDateTimeLastWeek' },
    { label: 'LocalDateTimeThisWeek', value: 'LocalDateTimeThisWeek' },
    { label: 'LocalDateTimeNextWeek', value: 'LocalDateTimeNextWeek' },
    { label: 'LocalDateTimeTwoWeeksAway', value: 'LocalDateTimeTwoWeeksAway' },
    { label: 'LocalDateTimeNextYear', value: 'LocalDateTimeNextYear' },
    { label: 'LocalDateTimeThisYear', value: 'LocalDateTimeThisYear' },
    { label: 'LocalDateTimeNextYear', value: 'LocalDateTimeNextYear' },
    { label: 'LocalDateTimeNextMonth', value: 'LocalDateTimeNextMonth' },
    { label: 'LocalDateTimeThisMonth', value: 'LocalDateTimeThisMonth' },
    { label: 'LocalDateTimeNextMonth', value: 'LocalDateTimeNextMonth' },
];



export const dateMetadata: Array<LabelValue> = [
    // Yesterday
    {
        label: 'Greater than or equal to local datetime yesterday',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMEYESTERDAY
    },
    {
        label: 'Less than local datetime today',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETODAY
    },

    // Today
    {
        label: 'Greater than or equal to local datetime today',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETODAY
    },
    {
        label: 'Less than local datetime tomorrow',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETOMORROW
    },

    // Tomorrow
    {
        label: 'Greater than or equal to local datetime tomorrow',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETOMORROW
    },
    {
        label: 'Less than local datetime after tomorrow',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMEDAYAFTERTOMORROW
    },

    // Last Week
    {
        label: 'Greater than or equal to local datetime last week',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMELASTWEEK
    },
    {
        label: 'Less than local datetime this week',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISWEEK
    },

    // This Week
    {
        label: 'Greater than or equal to local date time this week',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISWEEK
    },
    {
        label: 'Less than local date time next week',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTWEEK
    },

    // Next Week
    {
        label: 'Greater than or equal to local datetime next week',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMENEXTWEEK
    },
    {
        label: 'Less than local datetime two weeks away',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETWOWEEKSAWAY
    },

    // Next Year
    {
        label: 'Greater than or equal to local datetime next year',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMENEXTYEAR
    },
    {
        label: 'Less than next to next year',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_NEXTYEAR
    },

    // This Year
    {
        label: 'Greater than or equal to local datetime this year',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISYEAR
    },
    {
        label: 'Less than local datetime next year',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTYEAR
    },

    // Last Year
    {
        label: 'Greater than or equal to last year',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LASTYEAR
    },
    {
        label: 'Less than to this year',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISYEAR
    },

    // Next Month
    {
        label: 'Greater than or equal to next month',
        value: COMPLEXDATE_CONSTANT.GREATETHANOREQUAL_LOCALDATETIMENEXTMONTH
    },
    {
        label: 'Less than next month',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_NEXTMONTH
    },

    // This Month
    {
        label: 'Greater than or equal to local datetime this month',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LOCALDATETIMETHISMONTH
    },
    {
        label: 'Less than local datetime next month',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMENEXTMONTH
    },

    // Last Month
    {
        label: 'Greater than or equal to last month',
        value: COMPLEXDATE_CONSTANT.GREATERTHANOREQUAL_LASTMONTH
    },
    {
        label: 'Less than this month',
        value: COMPLEXDATE_CONSTANT.LESSTHAN_LOCALDATETIMETHISMONTH
    },
];

export let FILTER_CONSTANTS = {
    FROM_FILTER_DIALOG: 'FROMFILTERDIALOG',
    FROM_FILTER_GRID: 'FROMFILTERGRID'
}
