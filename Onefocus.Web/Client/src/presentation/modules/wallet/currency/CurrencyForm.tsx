import {useForm} from 'react-hook-form';
import {Switch, Text, Textarea} from '../../../components/form-controls';
import {Currency as DomainCurrency} from '../../../../domain/currency';
import {CurrencyFormInput} from './CurrencyFormInput';
import {WorkspaceRightPanel} from '../../../layouts/workspace';

export type CurrencyFormProps = {
    selectedCurrency: DomainCurrency | null | undefined;
    onSubmit: (data: CurrencyFormInput) => void;
    isPending?: boolean;
}

export const CurrencyForm = (props: CurrencyFormProps) => {
    const {control, handleSubmit} = useForm<CurrencyFormInput>({
        values: props.selectedCurrency ? {...props.selectedCurrency} :
            {
                id: undefined,
                name: '',
                shortName: '',
                isActive: false,
                isDefault: false,
                description: '',
            }
    });

    const isEditMode = Boolean(props.selectedCurrency);

    const buttons = [
        {
            id: 'btnSave',
            label: 'Save',
            icon: 'pi pi-save',
            onClick: () => {
                handleSubmit(props.onSubmit)();
            }
        }
    ];

    return (
        <WorkspaceRightPanel buttons={buttons} isPending={props.isPending}>
            <h3 className="mt-0 mb-5">{`${isEditMode ? 'Edit' : 'Add'} Currency`}</h3>
            <form>
                <Text control={control} name="name" label="Name" className="w-full of-w-max" rules={{
                    required: 'Name is required.',
                    maxLength: {value: 100, message: 'Name cannot exceed 100 characters.'}
                }}/>
                <Text control={control} name="shortName" label="Short name" className="w-full of-w-max" rules={{
                    required: 'Short name is required.',
                    maxLength: {value: 4, message: 'Name cannot exceed 4 characters.'}
                }}/>
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: {value: 255, message: 'Name cannot exceed 255 characters.'}
                }}/>
                {<Switch control={control} name="isDefault" label="Is default"
                         description="If this flag is 'enabled', it will be reset to 'disabled' for the other currencies."/>}
                {isEditMode && <Switch control={control} name="isActive" label="Is active"/>}
            </form>
        </WorkspaceRightPanel>
    );
};