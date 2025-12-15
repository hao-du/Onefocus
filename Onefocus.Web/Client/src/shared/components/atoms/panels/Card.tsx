import { Card as PrimeCard } from "primereact/card";
import { ReactNode } from "react";
import { BaseHtmlProps, BaseIdentityProps, ChildrenProps } from "../../../props/BaseProps";

interface CardOptions {
    index: number;
};

interface CardProps extends ChildrenProps, BaseIdentityProps, BaseHtmlProps {
    title?: string;
    subTitle?: string;
    header?: ReactNode;
    footer?: ReactNode | ((props: CardOptions) => React.ReactNode);
    index: number;
};

export const Card = (props: CardProps) => {
    return (
        <PrimeCard
            id={props.id}
            key={props.key}
            title={props.title}
            subTitle={props.subTitle}
            header={props.header}
            footer={
                typeof props.footer === "function"
                    ? props.footer({ index: props.index })
                    : props.footer
            }
            className={props.className}
        >
            {props.children}
        </PrimeCard>
    );
};