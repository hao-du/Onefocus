
const formatDateLocalSystem = (date?: Date | string | number) => {
    if (!date) return "";

    const d = date instanceof Date ? date : new Date(date);
    if (isNaN(d.getTime())) return ""; // Invalid date

    const newValue = new Intl.DateTimeFormat(navigator.language, {
        dateStyle: "short",
        timeStyle: "short",
    }).format(d);
    return newValue;
}

const formatCurrency = (value: number) => {
    return new Intl.NumberFormat(navigator.language, {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(value);
}

const getEmptyGuid = () => {
    return '00000000-0000-0000-0000-000000000000';
}

export {
    formatCurrency, formatDateLocalSystem, getEmptyGuid
};

