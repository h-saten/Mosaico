import { EventEmitter } from '@angular/core';
import { Token } from 'mosaico-wallet';
import { TokenDistribution } from '../../../../../projects/mosaico-wallet/src/lib/models/token-distribution';
import { Stage } from 'mosaico-project';

export class TokenDashboardService {

    private _reserveTokenAmount: number;
    private _reserveTokenAmountPercentage: number;
    private _overallSum: number;
    private _overallPercentage: number;
    private _token: Token;
    private _tokenDistributions: TokenDistribution[] = [];
    private _projectStages: { [key: string]: Stage[] } = {};
    private _isValid = true;

    public onTokenDistributionUpdated = new EventEmitter<TokenDistribution[]>(null);
    public onStagesUpdated = new EventEmitter<{ [key: string]: Stage[] }>(null);

    public constructor(token: Token) {
        this._token = token;
    }

    public get reserveTokenAmount(): number {
        return this._reserveTokenAmount;
    }

    public get reserveTokenAmountPercentage(): number {
        return this._reserveTokenAmountPercentage;
    }

    public get overallSum(): number {
        return this._overallSum;
    }

    public get overallPercentage(): number {
        return this._overallPercentage;
    }

    public get tokenDistribution(): TokenDistribution[] {
        return this._tokenDistributions;
    }

    public set tokenDistribution(d: TokenDistribution[]) {
        this._tokenDistributions = d;
        this.onTokenDistributionUpdated.emit(this._tokenDistributions);
    }

    public get isValid(): boolean {
        return this._isValid;
    }

    public updateTokenDistribution(index: number, amount: number): void {
        if(this._tokenDistributions && this._tokenDistributions[index]){
            this._tokenDistributions[index].tokenAmount = amount;
            this.recalculateReserveTokenAmount();
            this.calculateOverallPercentage();
            this.validateTableValues();
        }
    }

    public updateStage(id: string, stageIndex: number, amount: number): void {
        if(this._projectStages[id] && this._projectStages[id][stageIndex]) {
            this._projectStages[id][stageIndex].tokensSupply = amount;
            this.recalculateReserveTokenAmount();
            this.calculateOverallPercentage();
            this.validateTableValues();
        }
    }

    public setStages(id: string, stages: Stage[]): void {
        this._projectStages[id] = stages;
        this.onStagesUpdated.emit(this._projectStages);
    }

    public get stages(): { [key: string]: Stage[] } {
        return this._projectStages;
    }

    public recalculateReserveTokenAmount(): void {
        if (this._token && this._token.totalSupply > 0) {
            let sum = 0;
            this._reserveTokenAmount = this._token.totalSupply;
            if (this._tokenDistributions && this._tokenDistributions.length > 0) {
                sum += this._tokenDistributions.reduce((s, current) => s + current.tokenAmount, 0);
            }
            sum += this.getProjectStageSums();
            this._reserveTokenAmount = this._token.totalSupply - sum;
            this._reserveTokenAmountPercentage = +this.reserveTokenAmount * 100 / this._token.totalSupply;
        }
    }

    public getProjectStageSums(): number {
        let sum = 0;
        if (this._projectStages) {
            Object.keys(this._projectStages).forEach((projectId) => {
                const stages = this._projectStages[projectId];
                const projectSum = stages?.reduce((s, current) => s + current.tokensSupply, 0) ?? 0;
                if(projectSum > 0) {
                    sum += projectSum;
                }
            });
        }
        return sum;
    }

    public validateTableValues(): void {
        if (this._tokenDistributions && this._token && this._token.totalSupply > 0) {
            this.calculateOverallPercentage();
            let sum = this._tokenDistributions.reduce((s, current) => s + current.tokenAmount, 0);
            sum += this.getProjectStageSums();
            this._isValid = this._token.totalSupply >= sum;
        }
    }

    public calculateOverallPercentage(): void {
        if (this._tokenDistributions && this._token && this._token.totalSupply > 0) {
            this._overallSum = this._tokenDistributions.reduce((sum, current) => sum + current.tokenAmount, 0) + (this.reserveTokenAmount > 0 ? this.reserveTokenAmount : 0);
            this._overallSum += this.getProjectStageSums();
            this._overallPercentage = this.overallSum * 100 / this._token.totalSupply;
        }
        else {
            this._overallPercentage = 0;
            this._overallSum = 0;
        }
    }

    public deleteGroup(index: number): void {
        if(this._tokenDistributions && this._tokenDistributions[index]){
            this._tokenDistributions = this._tokenDistributions.filter((v, i) => i !== index);
        }
    }

    public deleteStage(projectId: string, index: number): void {
        if(this._projectStages && this._projectStages[projectId]) {
            this._projectStages[projectId] = this._projectStages[projectId]?.filter((s, i) => i !== index);
        }
    }

    public addDistributionGroup(t: TokenDistribution): void {
        this._tokenDistributions.push(t);
    }
}
