import React from 'react';

export interface IAppContextData {
    showHeader: boolean;
    showToolbar: boolean;
    transparentHeader: boolean;
    transparentToolbar: boolean;
    iconScale: number;
    zoomLevel: number;
    shape: string;
}

export interface IAppContext {
    value: IAppContextData;
    update: (delta: Partial<IAppContextData>) => void;
    toggleFrameMenu: () => void;
}

export const defaultAppContext: IAppContext = {
    value: {
        showHeader: true,
        showToolbar: false,
        transparentHeader: true,
        transparentToolbar: true,
        iconScale: 2.5,
        zoomLevel: 1,
        shape: 'inset(0%)',
    },
    update: () => { },
    toggleFrameMenu: () => { },
};

export const AppContext = React.createContext<IAppContext>(defaultAppContext);
