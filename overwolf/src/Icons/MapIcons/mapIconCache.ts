import { svgMapIconToImageBitmap } from '../../logic/svg';
import AlligatorIcon from './AlligatorIcon';
import BearIcon from './BearIcon';
import ChestIcon from './ChestIcon';
import DocumentIcon from './DocumentIcon';
import EssenceIcon from './EssenceIcon';
import EssenceMinusIcon from './EssenceMinusIcon';
import EssencePlusIcon from './EssencePlusIcon';
import FishIcon from './FishIcon';
import GoatIcon from './GoatIcon';
import LeopardIcon from './LeopardIcon';
import OreIcon from './OreIcon';
import PigIcon from './PigIcon';
import PlantIcon from './PlantIcon';
import PlayerIcon from './PlayerIcon';
import RabbitIcon from './RabbitIcon';
import SheepIcon from './SheepIcon';
import SkullIcon from './SkullIcon';
import TreeIcon from './TreeIcon';
import TurkeyIcon from './TurkeyIcon';
import UnknownIcon from './UnknownIcon';
import WolfIcon from './WolfIcon';

export type MapIconsCache = typeof createMapIconCache extends (...args: any[]) => Promise<infer T> ? T : never;

export async function createMapIconCache(scale: number) {
    return {
        player: await svgMapIconToImageBitmap(PlayerIcon, { fill: 'yellow', scale }),
        unknown: await svgMapIconToImageBitmap(UnknownIcon, { fill: 'red', scale }),

        ores: await svgMapIconToImageBitmap(OreIcon, { fill: 'green', scale }),
        iron: await svgMapIconToImageBitmap(OreIcon, { fill: 'saddlebrown', scale }),
        crystal: await svgMapIconToImageBitmap(OreIcon, { fill: 'lightcyan', scale }),
        gold: await svgMapIconToImageBitmap(OreIcon, { fill: 'gold', scale }),
        lodestone: await svgMapIconToImageBitmap(OreIcon, { fill: 'orangered', scale }),
        orichalcum: await svgMapIconToImageBitmap(OreIcon, { fill: 'darkred', scale }),
        saltpeter: await svgMapIconToImageBitmap(OreIcon, { fill: 'white', scale }),
        seeping_stone: await svgMapIconToImageBitmap(OreIcon, { fill: 'darkgray', scale }),
        silver: await svgMapIconToImageBitmap(OreIcon, { fill: 'silver', scale }),
        starmetal: await svgMapIconToImageBitmap(OreIcon, { fill: 'blue', scale }),

        woods: await svgMapIconToImageBitmap(TreeIcon, { fill: 'green', scale }),
        ironwood: await svgMapIconToImageBitmap(TreeIcon, { fill: 'gray', scale }),
        wyrdwood: await svgMapIconToImageBitmap(TreeIcon, { fill: 'darkblue', scale }),
        Briar: await svgMapIconToImageBitmap(TreeIcon, { fill: 'brown', scale }),

        fishing: await svgMapIconToImageBitmap(FishIcon, { fill: 'green', scale }),
        hotspot_broad: await svgMapIconToImageBitmap(FishIcon, { fill: 'blue', scale }),
        hotspot_rare: await svgMapIconToImageBitmap(FishIcon, { fill: 'yellow', scale }),
        hotspot_secret: await svgMapIconToImageBitmap(FishIcon, { fill: 'red', scale }),

        chests: await svgMapIconToImageBitmap(ChestIcon, { fill: 'saddlebrown', scale }),

        documents: await svgMapIconToImageBitmap(DocumentIcon, { fill: 'lightyellow', scale }),

        plants: await svgMapIconToImageBitmap(PlantIcon, { fill: 'lawngreen', scale }),

        monsters: await svgMapIconToImageBitmap(SkullIcon, { fill: 'green', scale }),

        turkey: await svgMapIconToImageBitmap(TurkeyIcon, { fill: 'brown', scale }),
        turkey_nest: await svgMapIconToImageBitmap(TurkeyIcon, { fill: 'dodgerblue', scale }),

        wolf: await svgMapIconToImageBitmap(WolfIcon, { fill: 'lightgray', scale }),
        wolf_elemental: await svgMapIconToImageBitmap(WolfIcon, { fill: 'dodgerblue', scale }),

        elk: await svgMapIconToImageBitmap(WolfIcon, { fill: 'saddlebrown', scale }),
        elk_elemental: await svgMapIconToImageBitmap(WolfIcon, { fill: 'dodgerblue', scale }),

        named: await svgMapIconToImageBitmap(SkullIcon, { fill: 'red', scale }),

        alligator: await svgMapIconToImageBitmap(AlligatorIcon, { fill: 'olive', scale }),

        rabbit: await svgMapIconToImageBitmap(RabbitIcon, { fill: 'white', scale }),

        bear: await svgMapIconToImageBitmap(BearIcon, { fill: 'saddlebrown', scale }),
        bear_elemental: await svgMapIconToImageBitmap(WolfIcon, { fill: 'dodgerblue', scale }),

        pig: await svgMapIconToImageBitmap(PigIcon, { fill: 'lightpink', scale }),
        boar: await svgMapIconToImageBitmap(PigIcon, { fill: 'darkgray', scale }),

        cow: await svgMapIconToImageBitmap(PigIcon, { fill: 'whitesmoke', scale }),
        bison: await svgMapIconToImageBitmap(PigIcon, { fill: 'darkorange', scale }),

        goat: await svgMapIconToImageBitmap(GoatIcon, { fill: 'lightgray', scale }),

        sheep: await svgMapIconToImageBitmap(SheepIcon, { fill: 'whitesmoke', scale }),

        leopard: await svgMapIconToImageBitmap(LeopardIcon, { fill: 'darkorange', scale }),
        lion: await svgMapIconToImageBitmap(LeopardIcon, { fill: 'darkgoldenrod', scale }),
        lynx: await svgMapIconToImageBitmap(LeopardIcon, { fill: 'slategray', scale }),

        essences: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'green', scale }),

        air_boid: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'whitesmoke', scale }),
        air_plant: await svgMapIconToImageBitmap(EssenceMinusIcon, { fill: 'whitesmoke', scale }),
        air_stone: await svgMapIconToImageBitmap(EssencePlusIcon, { fill: 'whitesmoke', scale }),

        death_boid: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'darkviolet', scale }),
        death_plant: await svgMapIconToImageBitmap(EssenceMinusIcon, { fill: 'darkviolet', scale }),
        death_stone: await svgMapIconToImageBitmap(EssencePlusIcon, { fill: 'darkviolet', scale }),

        earth_boid: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'saddlebrown', scale }),
        earth_plant: await svgMapIconToImageBitmap(EssenceMinusIcon, { fill: 'saddlebrown', scale }),
        earth_stone: await svgMapIconToImageBitmap(EssencePlusIcon, { fill: 'saddlebrown', scale }),

        fire_boid: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'orange', scale }),
        fire_plant: await svgMapIconToImageBitmap(EssenceMinusIcon, { fill: 'orange', scale }),
        fire_stone: await svgMapIconToImageBitmap(EssencePlusIcon, { fill: 'orange', scale }),

        life_boid: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'firebrick', scale }),
        life_plant: await svgMapIconToImageBitmap(EssenceMinusIcon, { fill: 'firebrick', scale }),
        life_stone: await svgMapIconToImageBitmap(EssencePlusIcon, { fill: 'firebrick', scale }),

        soul_boid: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'hotpink', scale }),
        soul_plant: await svgMapIconToImageBitmap(EssenceMinusIcon, { fill: 'hotpink', scale }),
        soul_stone: await svgMapIconToImageBitmap(EssencePlusIcon, { fill: 'hotpink', scale }),

        water_boid: await svgMapIconToImageBitmap(EssenceIcon, { fill: 'dodgerblue', scale }),
        water_plant: await svgMapIconToImageBitmap(EssenceMinusIcon, { fill: 'dodgerblue', scale }),
        water_stone: await svgMapIconToImageBitmap(EssencePlusIcon, { fill: 'dodgerblue', scale }),
    };
}
