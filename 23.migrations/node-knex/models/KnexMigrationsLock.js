const { Model } = require('objection');

class KnexMigrationsLock extends Model {

    static get tableName() {
        return 'knex_migrations_lock';
    }

    static get idColumn() {
        return 'index';
    }


    static get jsonSchema() {
        return {
            type: 'object',
            required: [],

            properties: {
                index: { type: 'number' },
                is_locked: { type: 'number' },
            
            }
        };
    }
    
}

module.exports = KnexMigrationsLock;
