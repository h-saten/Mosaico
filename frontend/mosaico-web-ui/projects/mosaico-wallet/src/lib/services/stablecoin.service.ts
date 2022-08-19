import { Injectable } from '@angular/core';
import { USDTMiniABIPolygon } from '../smart_contracts/usdt-mini.abi';
import { USDCMiniABIPolygon } from '../smart_contracts/usdc-mini.abi';

@Injectable({
    providedIn: 'root'
})
export class StablecoinSevice {
    abiMapPolygon = {
        'USDT': USDTMiniABIPolygon,
        'USDC': USDCMiniABIPolygon
    };

    public getAbi(network: string, ticker: string): any {
        if(network && network.toLowerCase() === 'polygon') {
            return this.abiMapPolygon[ticker];
        }
        return null;
    }
};