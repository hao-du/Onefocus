import { Calendar } from 'primereact/calendar';
import { useLocale } from '../../../hooks/locale/LocaleContext';
import { BaseHtmlProps, BaseIdentityProps } from '../../../props/BaseProps';

export interface DateTimeProps extends BaseIdentityProps, BaseHtmlProps {
    value?: Date | null;
    readOnly?: boolean;
    isPending?: boolean;
    invalid?: boolean;
    placeholder?: string;
    size?: 'normal' | 'small',
    onChange?: ((value: Date | null | undefined) => void);
    showTime?: boolean;
    showSeconds?: boolean;
    hourFormat?: '12' | '24';
    dateFormat?: string;
    minDate?: Date;
    maxDate?: Date;
    appendTo?: 'self' | HTMLElement | (() => HTMLElement);
};

export const DatePicker = (props: DateTimeProps) => {

    const { language, translate } = useLocale();
    return (
        <Calendar
            id={props.id}
            key={props.key}
            onChange={(e) => {
                if (props.onChange) {
                    props.onChange(e.value);
                }
            }}
            locale={language}
            invalid={props.invalid}
            value={props.value ? new Date(props.value) : undefined}
            showTime={props.showTime}
            hourFormat={props.hourFormat}
            minDate={props.minDate}
            maxDate={props.maxDate}
            placeholder={translate(props.placeholder)}
            dateFormat={props.dateFormat}
            showIcon={true}
            showSeconds={props.showSeconds}
            appendTo={props.appendTo ?? document.body}
            readOnlyInput={props.readOnly}
            disabled={props.isPending}
            className={`${props.className} ${props.size == 'small' ? 'p-inputtext-sm' : ''}`}
        />
    );
};