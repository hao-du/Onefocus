import { useForm } from 'react-hook-form';
import CounterpartyFormInput from './interfaces/CounterpartyFormInput';
import { WorkspaceRightPanel } from '../../../../shared/components/layouts/workspace';
import { Switch, Text, Textarea, TextOnly } from '../../../../shared/components/controls';

type CounterpartyFormProps = {
    selectedCounterparty: CounterpartyFormInput | null | undefined;
    onSubmit: (data: CounterpartyFormInput) => void;
    isPending?: boolean;
}

const CounterpartyForm = (props: CounterpartyFormProps) => {
    const { control, handleSubmit } = useForm<CounterpartyFormInput>({
        values: props.selectedCounterparty ? { ...props.selectedCounterparty } :
            {
                id: '',
                fullName: '',
                email: '',
                phoneNumber: '',
                isActive: true,
                description: ''
            }
    });

    const isEditMode = Boolean(props.selectedCounterparty);

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
            <h3 className="mt-0 mb-5"><TextOnly value={`${isEditMode ? 'Edit' : 'Add'} Counterparty`} /></h3>
            <form>
                <Text control={control} name="fullName" label="Full name" className="w-full of-w-max" rules={{
                    required: 'Full name is required.',
                    maxLength: { value: 100, message: 'Full name cannot exceed 100 characters.' }
                }} />
                <Text control={control} name="email" label="Email" className="w-full of-w-max" keyfilter="email" rules={{
                    required: 'Email is required.',
                    maxLength: { value: 254, message: 'Email cannot exceed 254 characters.' },
                    pattern: { value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: "Invalid email format" }
                }} />
                <Text control={control} name="phoneNumber" label="Phone number" className="w-full of-w-max" keyfilter="int" rules={{
                    required: 'Phone is required.',
                    maxLength: { value: 25, message: 'Phone cannot exceed 25 characters.' }
                }} />
                <Textarea control={control} name="description" label="Description" className="w-full of-w-max" rules={{
                    maxLength: { value: 255, message: 'Description cannot exceed 255 characters.' }
                }} />
                {isEditMode && <Switch control={control} name="isActive" label="Is active" />}
            </form>
        </WorkspaceRightPanel>
    );
};

export default CounterpartyForm;