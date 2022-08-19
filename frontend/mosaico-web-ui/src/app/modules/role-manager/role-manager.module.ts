import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RestrictedAreaComponent } from './components/restricted-area/restricted-area.component';
import { AccessGuard } from './guards';
import { RoleService } from './services';
import { defaultRoleFactory, RoleFactory } from './core/role-factory';
import { BehaviorType } from './core/behavior-type';

@NgModule({
    declarations: [RestrictedAreaComponent],
    imports: [CommonModule],
    exports: [
        RestrictedAreaComponent
    ]
})
export class RoleManagerModule {
    static forRoot(): ModuleWithProviders<RoleManagerModule> {
        return {
            ngModule: RoleManagerModule,
            providers: [
                AccessGuard,
                RoleService,
                {
                    provide: RoleFactory,
                    useFactory: defaultRoleFactory
                },
                {
                    provide: BehaviorType,
                    useValue: false
                }
            ]
        };
    }
}