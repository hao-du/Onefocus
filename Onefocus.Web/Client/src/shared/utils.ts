
export const normalizeDateTimeString = (input: string, showTime: boolean) => {
    if (!input || typeof input !== "string") return "";

    // Step 1: split into tokens
    const parts = input.trim().split(/[,\s]+/).filter(Boolean);
    let datePart = parts.find(p => p.includes("/") || p.includes("-")) || "";
    let timePart = parts.find(p => p.includes(":")) || "";
    const ampm = parts.find(p => /^am$|^pm$/i.test(p)); // detect AM/PM separately

    // Step 2: normalize date
    if (datePart) {
        const separator = datePart.includes("/") ? "/" : "-";
        const segments = datePart.split(separator).map(seg => seg.padStart(2, "0"));

        // Heuristic: if first part <=12 and second >12 → it's US MM/DD/YYYY, swap
        if (segments.length === 3) {
            const [a, b] = segments;
            if (+a <= 12 && +b > 12) {
                [segments[0], segments[1]] = [segments[1], segments[0]];
            }
        }

        datePart = segments.join("/");
    }

    // Step 3: normalize time
    if (timePart) {
        let [hour, minute = "00", second = ""] = timePart.split(":");

        let h = parseInt(hour, 10);
        const m = parseInt(minute, 10);
        const s = second ? parseInt(second, 10) : null;

        // Convert AM/PM → 24h
        if (ampm) {
            const isPM = /^pm$/i.test(ampm);
            const isAM = /^am$/i.test(ampm);

            if (isPM && h < 12) h += 12;
            if (isAM && h === 12) h = 0;
        }

        hour = h.toString().padStart(2, "0");
        minute = m.toString().padStart(2, "0");
        second = s !== null ? s.toString().padStart(2, "0") : "";

        timePart = [hour, minute, second].filter(Boolean).join(":");
    }

    // Step 4: merge
    return showTime
        ? `${datePart} ${timePart}`.trim()
        : datePart;
};

export const formatCurrency = (value: number) => {
    return new Intl.NumberFormat(navigator.language, {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    }).format(value);
};

export const getGuid = () => {
    return crypto.randomUUID();
};

export const getEmptyGuid = () => {
    return '00000000-0000-0000-0000-000000000000';
};

export const joinClassNames = (...classes: Array<string | undefined | null | false>) => {
    return classes.filter(Boolean).join(' ');
};

export const getRemUnit = () => parseFloat(getComputedStyle(document.documentElement).fontSize);