using FluentMigrator;

namespace TesteDBM.Data.Migrations;

[Migration(202412261753)]
public class CriarTabelaProdutos : Migration
{
    public override void Up()
    {
        Create.Table("Produtos")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Nome").AsString(100).NotNullable()
            .WithColumn("Descricao").AsString(int.MaxValue).Nullable()
            .WithColumn("Preco").AsDecimal(18, 2).NotNullable()
            .WithColumn("DataCadastro").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentDateTime);

        Create.UniqueConstraint("UC_Produtos_Nome").OnTable("Produtos").Column("Nome");
    }

    public override void Down()
    {
        Delete.Table("Produtos");
    }
}
