import { Card as PrimeCard } from "primereact/card";
import { PropsWithChildren, ReactNode } from "react";
import { BaseProps } from "../props";
import { CardOptions } from "./interfaces/CardOptions";

type CardProps = PropsWithChildren & BaseProps & {
    title?: string;
    subTitle?: string;
    header?: ReactNode;
    footer?: ReactNode | ((props: CardOptions) => React.ReactNode);
    index: number;
};

const Card = (props: CardProps) => {
    return (
        <PrimeCard
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
}

export default Card;